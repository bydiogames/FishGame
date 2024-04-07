using FishGame.Animation;
using FishGame.Animation.Animations;
using FishGame.Backgrounds;
using FishGame.Entities;
using FishGame.Interface;
using FishGame.Inventory;
using FishGame.Sound;
using FishGame.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace FishGame
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private CoroutineManager coroutineManager;

        private FishDB _fishDb;
        private FishJournal _fishJournal;

        private FishShadowAnimation _fishShadowAnimation;
        private Character _character;
        private Texture2D _gridTexture;
        private BackgroundManager _background;
        private Weather _weather;

        private SoundManager _soundManager;

        private EntityManager _entityManager;

        private MainUI _mainUI;
        private LocationUI locationUI;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _background = new BackgroundManager(Season.Spring, Location.Pond);

            this.Components.Add(coroutineManager = new CoroutineManager());

            _fishDb = new FishDB();
            this.Components.Add(_fishDb);

            this.Components.Add(_entityManager = new EntityManager(Content));

            _soundManager = new SoundManager();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _entityManager.Sb = _spriteBatch;

            this.Components.Add(_weather = new Weather(_background, _spriteBatch));

            _gridTexture = new Texture2D(GraphicsDevice, 1, 1);
            _gridTexture.SetData(new Color[] { Color.White });

            _background.Load(Content);
            _weather.Load(Content);

            _fishDb.LoadContent(Content);
            _fishJournal = new FishJournal(_fishDb);

            SpawnCharacter();

            _mainUI = new MainUI(_spriteBatch, _background, _fishJournal, _fishDb);
            Components.Add(_mainUI);
            _mainUI.Load(Content, GraphicsDevice);

            _mainUI.RequestGotoLocationScreen += () =>
            {
                _entityManager.DestroyEntity(_character);
                _fishShadowAnimation = null;

                _mainUI.Visible = false;
                _mainUI.HideFishPopup();
                locationUI.Visible = true;
            };

            locationUI = new LocationUI(_spriteBatch);
            Components.Add(locationUI);
            locationUI.Load(Content, GraphicsDevice);
            locationUI.Visible = false;

            locationUI.LocationChanged += (Location location) =>
            {
                _background.ChangeLocation(location);
                _mainUI.Visible = true;
                locationUI.Visible = false;

                SpawnCharacter();
            };

            _soundManager.Load(Content);
            _soundManager.UpdateSong(Season.Spring);
            _background.SeasonChanged += OnSeasonChanged;

            coroutineManager.Start(SeasonRoutine());
            coroutineManager.Start(ButtonPromptRoutine());
        }

        private void SpawnCharacter()
        {
            // Use the character's width and height to center them.  The character is moved up 4 px according to the sprite sheet spec, so subtract an extra 2 tiles from the height to center
            _character = new Character(new Vector2(EntityConstants.CharacterLocationXTiles, EntityConstants.CharacterLocationYTiles), _background);
            _character.Load(Content, _fishDb, _fishJournal);
            _character.ReelCompleted += OnReelCompletion;
            _character.PickupStarted += OnPickupStart;
            _character.PickupFinished += OnPickupFinish;
            _character.CastCompleted += OnCastCompletion;
            _character.Exclamation += OnExclamationStart;

            _entityManager.AddEntity(_character);
        }

        private IEnumerator<IWaitable> SeasonRoutine()
        {
            while (true) 
            {
                yield return new Wait(TimeSpan.FromMinutes(1));
                //yield return new WaitOnPredicate(() => _character != null);

                _background.NextSeason();
            }
        }

        private IEnumerator<IWaitable> ButtonPromptRoutine()
        {
            while(true)
            {
                if (_character != null && _mainUI.Visible)
                {
                    _mainUI.ShowButtonPromptForState(_character.State);
                }

                yield return null;
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            _background.Update();

            if (_fishShadowAnimation != null)
            {
                _fishShadowAnimation.Update(gameTime);
            }

            _background.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);

            // TODO: Add your drawing code here
            _background.Draw(_spriteBatch);

            if(_fishShadowAnimation != null)
            {
                _fishShadowAnimation.Draw(_spriteBatch);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        internal void OnReelCompletion(object sender, EventArgs e)
        {
            _fishShadowAnimation = null;
        }

        internal void OnPickupStart(object sender, EventArgs e)
        {
            _soundManager.PlayFishPickupSfx();
        }

        internal void OnPickupFinish(object sender, EventArgs e)
        {
            _mainUI.ShowFishPopup(_character.GetFish());
        }

        internal void OnExclamationStart(object sender, EventArgs e)
        {
            _soundManager.PlayExclamationSfx();
        }

        internal void OnCastCompletion(object sender, EventArgs e)
        {
            _fishShadowAnimation = new FishShadowAnimation(new Vector2(EntityConstants.FishShadowLocationXTiles, EntityConstants.FishShadowLocationYTiles));
            _fishShadowAnimation.Load(Content);
            _mainUI.HideFishPopup();
        }

        internal void OnSeasonChanged(object sender, SeasonChangedEventArgs e)
        {
            _soundManager.UpdateSong(e.Season);
        }
    }
}