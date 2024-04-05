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
        private TestBackgroundManager _background;

        private SoundManager _soundManager;

        private EntityManager _entityManager;

        private MainUI _mainUI;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _background = new TestBackgroundManager(Season.Spring, Location.Pond);

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

            _gridTexture = new Texture2D(GraphicsDevice, 1, 1);
            _gridTexture.SetData(new Color[] { Color.White });

            _background.Load(Content);

            _fishDb.LoadContent(Content);
            _fishJournal = new FishJournal(_fishDb);

            // Use the character's width and height to center them.  The character is moved up 4 px according to the sprite sheet spec, so subtract an extra 2 tiles from the height to center
            _character = new Character(new Vector2(EntityConstants.CharacterLocationXTiles, EntityConstants.CharacterLocationYTiles), _background);
            _character.Load(Content, _fishDb, _fishJournal);
            _character.ReelCompleted += OnReelCompletion;
            _character.PickupStarted += OnPickupStart;
            _character.CastCompleted += OnCastCompletion;
            _character.Exclamation += OnExclamationStart;

            _entityManager.AddEntity(_character);

            _mainUI = new MainUI(_spriteBatch, _background, _fishJournal, _fishDb);
            Components.Add(_mainUI);
            _mainUI.Load(Content, GraphicsDevice);

            _soundManager.Load(Content);

            coroutineManager.Start(Routine());
        }

        private static IEnumerator<IWaitable> Routine()
        {
            yield return new Wait(TimeSpan.FromSeconds(5));
            Console.Out.WriteLine("Routine finished!");
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

        internal void OnExclamationStart(object sender, EventArgs e)
        {
            _soundManager.PlayExclamationSfx();
        }

        internal void OnCastCompletion(object sender, EventArgs e)
        {
            _fishShadowAnimation = new FishShadowAnimation(new Vector2(EntityConstants.FishShadowLocationXTiles, EntityConstants.FishShadowLocationYTiles));
            _fishShadowAnimation.Load(Content);
        }
    }
}