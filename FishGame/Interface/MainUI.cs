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
        private TestBackgroundManager _background;
        private FishDB _fishDb;
        private FishJournal _fishJournal;

        public MainUI(SpriteBatch spriteBatch, TestBackgroundManager background, FishJournal fishJournal, FishDB fishDb)
        {
            _spriteBatch = spriteBatch;
            _background = background;
            _fishJournal = fishJournal;
            _fishDb = fishDb;
        }

        private Texture2D _mainUiOverlayTex, _mainUiTilesTex;
        private Texture2D _fishAllTex, _fishAllMissingTex;

        private Texture2D _seasonCardsTex;

        private SpriteFont _font;

        private Texture2D _square;

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

            _seasonCardsTex = content.Load<Texture2D>("Season_Cards__Tiles");

            _font = content.Load<SpriteFont>("gamefont");

            _square = new Texture2D(graphics, 1, 1);
            _square.SetData<Color>(new Color[] { Color.White });
        }

        int IDrawable.DrawOrder => 50;

        bool IDrawable.Visible => true;

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
                hoverTime = TimeSpan.Zero;
            }

            _spriteBatch.End();
        }

        void IGameComponent.Initialize()
        {
        }
    }
}
