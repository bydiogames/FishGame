using FishGame.Backgrounds;
using FishGame.Entities;
using FishGame.Inventory;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FishGame.Interface
{
    internal class MainUI : IGameComponent, IDrawable
    {
        private SpriteBatch _spriteBatch;
        private BackgroundManager _background;
        private FishDB _fishDb;
        private FishJournal _fishJournal;

        public event Action RequestGotoLocationScreen;

        public MainUI(SpriteBatch spriteBatch, BackgroundManager background, FishJournal fishJournal, FishDB fishDb)
        {
            _spriteBatch = spriteBatch;
            _background = background;
            _fishJournal = fishJournal;
            _fishDb = fishDb;
        }

        private Texture2D _mainUiOverlayTex, _mainUiTilesTex;
        private Texture2D _fishAllTex, _fishAllMissingTex;

        private Texture2D _seasonCardsTex;

        private Texture2D _caughtFishTex;
        private float _caughtFishTextWidth;

        private Texture2D _buttonsTex;

        private SpriteFont _font;
        private SpriteFont _popupFont;

        private Texture2D _square;

        private struct ButtonTile
        {
            public Rectangle srcRec;
            public Rectangle destRec;
        };

        private static readonly ButtonTile?[] _buttonForState = new ButtonTile?[]
        {
            new ButtonTile
            {
                srcRec = new Rectangle(498, 226, 44, 12),
                destRec = new Rectangle(40, 426, 44, 12),
            },
            null,
            null,
            new ButtonTile
            {
                srcRec = new Rectangle(498, 226, 44, 12),
                destRec = new Rectangle(40, 426, 44, 12),
            },
            null,
            null,
            new ButtonTile
            {
                srcRec = new Rectangle(498, 226, 44, 12),
                destRec = new Rectangle(40, 426, 44, 12),
            },
        };

        private static readonly int[] SeasonCardIdxForSeason = new int[] 
        { 
            1,
            2,
            3,
            0
        };

        public void Load(ContentManager content, GraphicsDevice graphics)
        {
            // Load fish texture maps.
            {
                _fishAllTex = content.Load<Texture2D>("fish_all");
                _fishAllMissingTex = content.Load<Texture2D>("inv_fish_shadow");
            }

            {
                _mainUiOverlayTex = content.Load<Texture2D>("Main_ui__Ui_tiles");
                _mainUiTilesTex = content.Load<Texture2D>("Main_ui__Tiles");
            }

            _caughtFishTex = content.Load<Texture2D>("Main_ui__Fish_caught_popup");

            _seasonCardsTex = content.Load<Texture2D>("Season_Cards__Tiles");

            _font = content.Load<SpriteFont>("gamefont");
            _popupFont = content.Load<SpriteFont>("popup_font");

            _square = new Texture2D(graphics, 1, 1);
            _square.SetData<Color>(new Color[] { Color.White });

            _buttonsTex = content.Load<Texture2D>("tilemap_white_packed");
        }

        int IDrawable.DrawOrder => 50;

        private bool visible = true;

        public bool Visible
        {
            get => visible; set
            {
                visible = value;
                if (VisibleChanged != null)
                    VisibleChanged(this, EventArgs.Empty);
            }
        }

        private CharacterState _buttonPromptState;

        public void ShowButtonPromptForState(CharacterState state)
        {
            _buttonPromptState = state;
        }

        public event EventHandler<EventArgs> DrawOrderChanged;

        public event EventHandler<EventArgs> VisibleChanged;

        private int? lastHoverIdx;
        private TimeSpan hoverTime;

        private void DrawToolTip(string tooltip)
        {
            var mouseState = Mouse.GetState();

            Vector2 stringDim = _font.MeasureString(tooltip);
            _spriteBatch.Draw(
                _square, 
                new Rectangle(
                    mouseState.Position - new Point(0, (int)stringDim.Y) - new Point(2, 2), 
                    stringDim.ToPoint() + new Point(2, 2)
                ), 
                Color.Brown
            );
            _spriteBatch.DrawString(_font, tooltip, mouseState.Position.ToVector2() - new Vector2(0, stringDim.Y) * 1f, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
        }

        private bool _showFishPopup;
        private FishRecord _caughtFish;

        public void ShowFishPopup(FishRecord fish)
        {
            _showFishPopup = true;
            _caughtFish = fish;
            _caughtFishTextWidth = _popupFont.MeasureString(fish.Name).X;
        }

        public void HideFishPopup()
        {
            _showFishPopup = false;
        }

        private void DrawFishPopup()
        {
            _spriteBatch.Draw(_caughtFishTex, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 2f, SpriteEffects.None, 0);
            float textLocationX = (EntityConstants.FishPopupLocationX + ((EntityConstants.FishPopupWidthPx - _caughtFishTextWidth) / 2));
            _spriteBatch.DrawString(_popupFont, _caughtFish.Name, new Vector2(textLocationX, EntityConstants.FishPopupLocationY), Color.SaddleBrown, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
        }

        void IDrawable.Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);

            _spriteBatch.Draw(_mainUiTilesTex, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 2f, SpriteEffects.None, 0);
            _spriteBatch.Draw(_mainUiOverlayTex, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 2f, SpriteEffects.None, 0);

            var uiUpperLeft = _spriteBatch.GraphicsDevice.Viewport.Bounds.Size.ToVector2() * new Vector2(0.5f, 0.1f);

            _fishJournal.Draw(
                _spriteBatch,
                _fishAllMissingTex,
                _fishAllTex,
                uiUpperLeft,
                _background.GetSeason(),
                _background.GetLocation()
            );

            var mouseState = Mouse.GetState();

            ref var buttonTile = ref _buttonForState[(int)_buttonPromptState];
            if (buttonTile.HasValue)
            {
                _spriteBatch.Draw(_buttonsTex, buttonTile.Value.destRec, buttonTile.Value.srcRec, Color.White);
            }

            bool anyHover = false;

            {
                var seasonCardIdx = SeasonCardIdxForSeason[System.Numerics.BitOperations.Log2((uint)_background.GetSeason())];
                var seasonCardXOffset = seasonCardIdx * 32 * 2;

                var seasonCardSrcRect = new Rectangle(seasonCardXOffset, 0, 32, 16 * 3);
                var seasonCardDstRect = new Rectangle(160 * 2, 185 * 2, 32 * 2, 16 * 3 * 2 + 1);
                _spriteBatch.Draw(_seasonCardsTex, seasonCardDstRect, seasonCardSrcRect, Color.White);

                if (seasonCardDstRect.Contains(mouseState.Position))
                {
                    anyHover = true;
                    hoverTime += gameTime.ElapsedGameTime;

                    if (hoverTime > TimeSpan.FromSeconds(1))
                    {
                        DrawToolTip(_background.GetSeason().GetName());
                    }
                }
            }

            if (!anyHover)
            {
                var hoverIdx = _fishJournal.QueryHover(uiUpperLeft, mouseState.Position.ToVector2());

                if (hoverIdx.HasValue)
                {
                    anyHover = true;

                    if (!lastHoverIdx.HasValue || lastHoverIdx.Value != hoverIdx.Value)
                    {
                        hoverTime = TimeSpan.Zero;
                        lastHoverIdx = hoverIdx.Value;
                    }

                    hoverTime += gameTime.ElapsedGameTime;

                    if (hoverTime > TimeSpan.FromSeconds(1))
                    {
                        ref FishRecord record = ref _fishDb.GetFishById(hoverIdx.Value);
                        ref FishInventoryEntry entry = ref _fishJournal.GetInvSlot(hoverIdx.Value);

                        string name = entry.HasCollected ? record.Name : "???";
                        DrawToolTip(name);
                    }
                }
                else
                {
                    lastHoverIdx = null;
                }
            }

            if (!anyHover)
            {
                var tileDim = new Point(16, 16) * new Point(2);
                var mapTileLocation = new Point(11, 10) * tileDim;
                if (new Rectangle(mapTileLocation, tileDim).Contains(mouseState.Position))
                {
                    anyHover = true;
                    hoverTime += gameTime.ElapsedGameTime;
                    if (hoverTime > TimeSpan.FromSeconds(1))
                    {
                        DrawToolTip("Goto Map");
                    }

                    if (mouseState.LeftButton == ButtonState.Pressed)
                    {
                        RequestGotoLocationScreen();
                    }
                }
            }

            if (!anyHover)
            {
                hoverTime = TimeSpan.Zero;
            }

            if(_showFishPopup)
            {
                DrawFishPopup();
            }

            _spriteBatch.End();
        }

        void IGameComponent.Initialize()
        {
        }
    }
}
