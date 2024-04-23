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

        private Screen _currentScreen;

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
            _currentScreen = new BydiogameUI(this, GraphicsDevice, Content, coroutineManager);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _entityManager.Sb = _spriteBatch;

            this.Components.Add(_weather = new Weather(_background, _spriteBatch));
            _weather.Visible = false;

            _gridTexture = new Texture2D(GraphicsDevice, 1, 1);
            _gridTexture.SetData(new Color[] { Color.White });

            _background.Load(Content);
            _background.SeasonChanged += OnSeasonChanged;
            _weather.Load(Content);

            _fishDb.LoadContent(Content);
            _fishJournal = new FishJournal(_fishDb);

            _soundManager.Load(Content);
            _currentScreen.LoadContent();
        }

        public void LoadMainUI()
        {
            MainUI mainUI = new MainUI(this, GraphicsDevice, Content, _background, _fishJournal, _fishDb);
            mainUI.PlayMusic += OnPlayMusic;
            mainUI.MuteMusic += OnMuteMusic;
            mainUI.PlaySfx += OnPlaySfx;
            mainUI.MuteSfx += OnMuteSfx;
            
            _currentScreen = mainUI;
            _currentScreen.LoadContent();
            SpawnCharacter();
            _weather.Visible = true;
            _soundManager.UpdateSong(_background.GetSeason());
            coroutineManager.Start(SeasonRoutine());
            //coroutineManager.Start(ButtonPromptRoutine());
        }

        public void LoadLocationScreen()
        {
            _entityManager.DestroyEntity(_character);
            _fishShadowAnimation = null;
            _currentScreen = new LocationUI(this, GraphicsDevice, Content);
            _currentScreen.LoadContent();
        }

        public void LoadSeasonScreen()
        {
            _entityManager.DestroyEntity(_character);
            _fishShadowAnimation = null;
            _currentScreen = new SeasonUI(this, GraphicsDevice, Content);
            _currentScreen.LoadContent();
        }

        public void ChangeLocation(Location location)
        {
            _background.ChangeLocation(location);
        }

        private void LoadBydioGameUI()
        {
            _currentScreen = new BydiogameUI(this, GraphicsDevice, Content, coroutineManager);
            _currentScreen.LoadContent();
        }

        private void _mainUI_MuteSfx(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void SpawnCharacter()
        {
            // Use the character's width and height to center them.  The character is moved up 4 px according to the sprite sheet spec, so subtract an extra 2 tiles from the height to center
            _character = new Character(new Vector2(EntityConstants.CharacterLocationXTiles, EntityConstants.CharacterLocationYTiles), _background);
            _character.Load(Content, _fishDb, _fishJournal);
            _character.ReelStarted += OnReelStarted;
            _character.ReelFinished += OnReelFinished;
            _character.PickupStarted += OnPickupStarted;
            _character.PickupFinished += OnPickupFinished;
            _character.CastStarted += OnCastStarted;
            _character.CastFinished += OnCastFinished;
            _character.Exclamation += OnExclamation;

            _entityManager.AddEntity(_character);
        }

        private IEnumerator<IWaitable> SeasonRoutine()
        {
            while (true) 
            {
                yield return new Wait(TimeSpan.FromMinutes(1));
                _background.NextSeason();
            }
        }

        // TODO: Refactor this to be on the MainUI
/*        private IEnumerator<IWaitable> ButtonPromptRoutine()
        {
            while(true)
            {
                if (_character != null)
                {
                    _mainUI.ShowButtonPromptForState(_character.State);
                }

                yield return null;
            }
        }*/

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            _currentScreen.Update(gameTime);
            _background.Update();

            if (_fishShadowAnimation != null)
            {
                _fishShadowAnimation.Update(gameTime);
            }

            base.Update(gameTime);
        }

        public void ShowCredits(object sender,  EventArgs e)
        {
            _currentScreen = new Credits(this, GraphicsDevice, Content, _spriteBatch, coroutineManager);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);

            _currentScreen.Draw(gameTime, _spriteBatch);

            if(_fishShadowAnimation != null)
            {
                _fishShadowAnimation.Draw(_spriteBatch);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        internal void OnReelStarted(object sender, EventArgs e)
        {
            _soundManager.PlayReelSfx();
        }

        internal void OnReelFinished(object sender, EventArgs e)
        {
            _fishShadowAnimation = null;
        }

        internal void OnPickupStarted(object sender, EventArgs e)
        {
            _soundManager.PlayFishPickupSfx();
        }

        internal void OnPickupFinished(object sender, EventArgs e)
        {
        }

        internal void OnExclamation(object sender, EventArgs e)
        {
            _soundManager.PlayExclamationSfx();
        }

        internal void OnCastStarted(object sender, EventArgs e)
        { 
            _soundManager.PlayCastSfx();
        }

        internal void OnCastFinished(object sender, EventArgs e)
        {
            _fishShadowAnimation = new FishShadowAnimation(new Vector2(EntityConstants.FishShadowLocationXTiles, EntityConstants.FishShadowLocationYTiles));
            _fishShadowAnimation.Load(Content);
        }

        internal void OnSeasonChanged(object sender, SeasonChangedEventArgs e)
        {
            _soundManager.UpdateSong(e.Season);
        }

        internal void OnPlayMusic(object sender, EventArgs e)
        {
            _soundManager.PlayMusic();
        }

        internal void OnMuteMusic(object sender, EventArgs e)
        {
            _soundManager.MuteMusic();
        }

        internal void OnPlaySfx(object sender, EventArgs e)
        {
            _soundManager.PlaySfx();
        }

        internal void OnMuteSfx(object sender, EventArgs e)
        {
            _soundManager.MuteSfx();
        }
    }
}