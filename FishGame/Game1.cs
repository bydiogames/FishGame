using FishGame.Animation;
using FishGame.Animation.Animations;
using FishGame.Backgrounds;
using FishGame.Entities;
using FishGame.Inventory;
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

        private Texture2D _mainUiOverlayTex, _mainUiTilesTex;
        private Texture2D _fishAllTex, _fishAllMissingTex;

        private EntityManager _entityManager;

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

            // Load fish texture maps.
            {
                _fishAllTex = Content.Load<Texture2D>("fish_all");
                _fishAllMissingTex = Content.Load<Texture2D>("inv_fish_shadow");
            }

            {
                _mainUiOverlayTex = Content.Load<Texture2D>("Main_ui__Ui_tiles");
                _mainUiTilesTex = Content.Load<Texture2D>("Main_ui__Tiles");
            }

            // Use the character's width and height to center them.  The character is moved up 4 px according to the sprite sheet spec, so subtract an extra 2 tiles from the height to center
            _character = new Character(new Vector2(EntityConstants.CharacterLocationXTiles, EntityConstants.CharacterLocationYTiles), _background, OnReelCompletion, OnCastCompletion);
            _character.Load(Content, _fishDb);

            _entityManager.AddEntity(_character);

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
            _character.Update(gameTime);
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
            //DrawGrid();

            // TODO: Add your drawing code here
            _background.Draw(_spriteBatch);
            _character.Draw(_spriteBatch);

            if(_fishShadowAnimation != null)
            {
                _fishShadowAnimation.Draw(_spriteBatch);
            }

            {
                _spriteBatch.Begin();

                _spriteBatch.Draw(_mainUiTilesTex, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 2f, SpriteEffects.None, 0);
                _spriteBatch.Draw(_mainUiOverlayTex, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 2f, SpriteEffects.None, 0);

                _fishJournal.Draw(
                    _spriteBatch,
                    _fishAllMissingTex,
                    _fishAllTex, 
                    GraphicsDevice.Viewport.Bounds.Size.ToVector2() * new Vector2(0.5f, 0.1f),
                    _background.GetSeason(),
                    _background.GetLocation()
                );
                _spriteBatch.End();
            }

            base.Draw(gameTime);
        }


        internal void OnReelCompletion()
        {
            _fishShadowAnimation = null;
        }

        internal void OnCastCompletion()
        {
            _fishShadowAnimation = new FishShadowAnimation(new Vector2(EntityConstants.FishShadowLocationXTiles, EntityConstants.FishShadowLocationYTiles));
            _fishShadowAnimation.Load(Content);
        }


        private void DrawGrid()
        {
            _spriteBatch.Begin();
            for(int i = 0; i < EntityConstants.ScreenWidthTiles; i++)
            {
                for(int j = 0; j < EntityConstants.ScreenHeightTiles; j++)
                {
                    _spriteBatch.Draw(_gridTexture, new Rectangle(i * EntityConstants.TileWidthPx + 1, j * EntityConstants.TileHeightPx + 1, EntityConstants.TileWidthPx - 2, EntityConstants.TileHeightPx - 2), Color.CornflowerBlue);
                }
            }
            _spriteBatch.End();
        }
    }
}