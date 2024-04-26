using FishGame.Backgrounds;
using FishGame.Entities;
using FishGame.Inputs;
using FishGame.Inventory;
using FishGame.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace FishGame.Interface
{
    internal class MainUI : Screen, IGameComponent
    {
        private BackgroundManager _background;
        private FishDB _fishDb;
        private FishJournal _fishJournal;
        private ToolTip _toolTip;
        private CoroutineManager _coroutineManager;

        public event EventHandler PlaySfx;
        public event EventHandler MuteSfx;
        public event EventHandler PlayMusic;
        public event EventHandler MuteMusic;

        public MainUI(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, 
            BackgroundManager background, FishJournal fishJournal, FishDB fishDb, CoroutineManager coroutineManager) : base(game, graphicsDevice, content)
        {
            _background = background;
            _fishJournal = fishJournal;
            _fishDb = fishDb;
            _toolTip = new ToolTip();
            _coroutineManager = coroutineManager;

            _background.SeasonChanged += OnSeasonChanged;
        }

        private Texture2D _mainUiTilesTex;
        private Texture2D _fishAllTex, _fishAllMissingTex;

        private Texture2D _seasonCardsTex;

        private Texture2D _caughtFishTex;
        private float _caughtFishTextWidth;

        private Texture2D _buttonsTex;

        private TextureToggleButton _sfxButton;
        private TextureToggleButton _musicButton;
        private TextureButton _locationButton;
        private TextureButton _seasonButton;

        private TextButton _creditsButton;

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
                destRec = new Rectangle(40, 426, 52, 16),
            },
            null,
            null,
            new ButtonTile
            {
                srcRec = new Rectangle(498, 226, 44, 12),
                destRec = new Rectangle(40, 426, 52, 16),
            },
            null,
            null,
            new ButtonTile
            {
                srcRec = new Rectangle(498, 226, 44, 12),
                destRec = new Rectangle(40, 426, 52, 16),
            },
        };

        private static readonly int[] SeasonCardIdxForSeason = new int[] 
        { 
            1,
            2,
            3,
            0
        };

        public override void LoadContent()
        {
            // Load fish texture maps.
            {
                _fishAllTex = _content.Load<Texture2D>("fish_all");
                _fishAllMissingTex = _content.Load<Texture2D>("inv_fish_shadow");
            }

            {
                _mainUiTilesTex = _content.Load<Texture2D>("Main_ui__Tiles");
            }

            _caughtFishTex = _content.Load<Texture2D>("Main_ui__Fish_caught_popup");

            _seasonCardsTex = _content.Load<Texture2D>("Season_Cards__Tiles");

            _font = _content.Load<SpriteFont>("gamefont");
            _popupFont = _content.Load<SpriteFont>("popup_font");

            // Load sound menu button textures
            {
                Texture2D onTexture = _content.Load<Texture2D>("Speaker");
                Texture2D offTexture = _content.Load<Texture2D>("Speaker-Crossed");
                _sfxButton = new TextureToggleButton(onTexture, offTexture, new Rectangle(_sfxPosX + (int)(_font.MeasureString("SFX").X / 2) + 4, _menuPosY, 16, 16)) ;
                _musicButton = new TextureToggleButton(onTexture, offTexture, new Rectangle(_musicPosX + (int)(_font.MeasureString("Music").X / 2) + 4, _menuPosY, 16, 16));

                _sfxButton.ToggleOn += ToggleSfxOn;
                _sfxButton.ToggleOff += ToggleSfxOff;
                _musicButton.ToggleOn += ToggleMusicOn;
                _musicButton.ToggleOff += ToggleMusicOff;
            }

            // Load credits button
            {
                _creditsButton = new TextButton(_font, new Vector2(_creditsPosX, _menuPosY), "Credits", 0.5f);
                _creditsButton.Clicked += CreditsButtonClicked;
            }

            // Load location button
            {
                Texture2D compassTexture = _content.Load<Texture2D>("compass");
                var tileDim = new Point(16, 16) * new Point(2);
                var mapTileLocation = new Point(11, 10) * tileDim;
                var texSize = new Point(compassTexture.Width, compassTexture.Height);
                _locationButton = new TextureButton(compassTexture, new Rectangle(mapTileLocation, tileDim), new Rectangle(Point.Zero, texSize));
                _locationButton.Pressed += OnLocationButtonPressed;
            }

            // Start coroutine for button prompt ui
            _coroutineManager.Start(ButtonPromptRoutine());

            // Season Button
            UpdateSeasonButton();
            _seasonButton.Pressed += OnSeasonButtonPressed;

            _toolTip.Load(_content, _graphicsDevice);

            _buttonsTex = _content.Load<Texture2D>("tilemap_white_packed");
        }

        public event EventHandler ShowCredits;
        private void CreditsButtonClicked(object sender, EventArgs e)
        {
            ShowCredits?.Invoke(this, EventArgs.Empty);
        }

        private void ToggleMusicOn(object sender, EventArgs e)
        {
            _playMusic = true;
            PlayMusic?.Invoke(this, EventArgs.Empty);
        }

        private void ToggleMusicOff(object sender, EventArgs e)
        {
            _playMusic = false;
            MuteMusic?.Invoke(this, EventArgs.Empty);
        }

        private void ToggleSfxOn(object sender, EventArgs e)
        {
            _playSFX = true;
            PlaySfx?.Invoke(this, EventArgs.Empty);
        }

        private void ToggleSfxOff(object sender, EventArgs e)
        {
            _playSFX = false;
            MuteSfx?.Invoke(this, EventArgs.Empty);
        }

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

        private void DrawFishPopup(SpriteBatch spriteBatch)
        {
            float scalingFactor = 1f;
            if(_caughtFishTextWidth > (EntityConstants.FishPopupWidthPx - 18))
            {
                scalingFactor = (EntityConstants.FishPopupWidthPx - 18) / _caughtFishTextWidth;
            }
            spriteBatch.Draw(_caughtFishTex, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 2f, SpriteEffects.None, 0);
            float textLocationX = (EntityConstants.FishPopupLocationX + ((EntityConstants.FishPopupWidthPx - (_caughtFishTextWidth * scalingFactor)) / 2));
            spriteBatch.DrawString(_popupFont, _caughtFish.Name, new Vector2(textLocationX, EntityConstants.FishPopupLocationY), Color.SaddleBrown, 0, Vector2.Zero, scalingFactor, SpriteEffects.None, 0);
        }

        private void OnSeasonChanged(object sender, SeasonChangedEventArgs e)
        {
            _seasonElapsedTime = 0;
            UpdateSeasonButton();
        }

        private void UpdateSeasonButton()
        {
            var seasonCardIdx = SeasonCardIdxForSeason[System.Numerics.BitOperations.Log2((uint)_background.GetSeason())];
            var seasonCardXOffset = seasonCardIdx * 32 * 2;

            var seasonCardSrcRect = new Rectangle(seasonCardXOffset, 0, 32, 16 * 3);
            var seasonCardDstRect = new Rectangle(160 * 2, 185 * 2, 32 * 2, 16 * 3 * 2 + 1);
            _seasonButton = new TextureButton(_seasonCardsTex, seasonCardDstRect, seasonCardSrcRect);
        }

        private float _seasonElapsedTime = 0;
        private void DrawDate(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _seasonElapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            spriteBatch.DrawString(_font, $"{_background.GetSeason()} {(int)(1 + (_seasonElapsedTime/(60f/28)))}", 
                new Vector2(48, 48), Color.White, 0, Vector2.Zero, 0.75f, SpriteEffects.None, 0);
        }

        // TODO: Refactor this to be on the MainUI
        private IEnumerator<IWaitable> ButtonPromptRoutine()
        {
            while (true)
            {
                    ShowButtonPromptForState(_game.GetCharacterState());

                yield return null;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _background.Draw(spriteBatch);
            spriteBatch.Draw(_mainUiTilesTex, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 2f, SpriteEffects.None, 0);

            var uiUpperLeft = spriteBatch.GraphicsDevice.Viewport.Bounds.Size.ToVector2() * new Vector2(0.5f, 0.07f);

            _fishJournal.Draw(
                spriteBatch,
                _fishAllMissingTex,
                _fishAllTex,
                uiUpperLeft,
                _background.GetSeason(),
                _background.GetLocation()
            );

            spriteBatch.DrawString(_font, $"Completion: {(int)(100 * _fishJournal.GetCompletionPercent())}%", new Vector2(448, 372), Color.White);

            var mouseState = Mouse.GetState();

            ref var buttonTile = ref _buttonForState[(int)_buttonPromptState];
            if (buttonTile.HasValue)
            {
                spriteBatch.Draw(_buttonsTex, buttonTile.Value.destRec, buttonTile.Value.srcRec, Color.White);
                spriteBatch.DrawString(_font, "SPACE", new Vector2(buttonTile.Value.destRec.X + 4, buttonTile.Value.destRec.Y), Color.Gray, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
            }

            bool anyHover = false;
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
                        _toolTip.Draw(name, spriteBatch);
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

            // Location button
            _locationButton.Draw(spriteBatch);
            if (_locationButton.IsHovering)
            {
                _toolTip.Draw("Go to Map", spriteBatch);
            }

            _seasonButton.Draw(spriteBatch);
            if (_seasonButton.IsHovering)
            {
                _toolTip.Draw(_background.GetSeason().GetName(), spriteBatch);
            }

            if (_showFishPopup)
            {
                DrawFishPopup(spriteBatch);
            }

            DrawDate(gameTime, spriteBatch);
            DrawMenu(spriteBatch);
        }

        private bool _playMusic = true;
        private bool _playSFX = true;
        private void ToggleSfx(object sender, EventArgs e)
        {
            _playSFX = !_playSFX;
        }

        private void ToggleMusic(object sender, EventArgs e)
        {
            _playSFX = !_playMusic;
        }

        private void OnLocationButtonPressed(object sender, EventArgs e)
        {
            _game.LoadLocationScreen();
        }

        private void OnSeasonButtonPressed(object sender, EventArgs e)
        {
            _game.LoadSeasonScreen();
        }

        private void DrawMenu(SpriteBatch spriteBatch)
        {
            // Music toggle
            {
                spriteBatch.DrawString(_font, "Music", new Vector2(_musicPosX, _menuPosY), Color.White,
                    0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
                _musicButton.Draw(spriteBatch);
            }

            // SFX toggle
            {
                spriteBatch.DrawString(_font, "SFX", new Vector2(_sfxPosX, _menuPosY), Color.White,
                    0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
                _sfxButton.Draw(spriteBatch);
            }

            // Credits button
            {
                _creditsButton.Draw(spriteBatch);
            }
        }


        private int _musicPosX = 592;
        private int _sfxPosX = 672;
        private int _menuPosY = 448;

        private int _creditsPosX = 520;

        void IGameComponent.Initialize()
        {
        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}
