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
using System.Linq;
using System.Xml.Serialization;

namespace FishGame
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private CoroutineManager coroutineManager;

        private FishDB _fishDb;
        private FishJournal fishJournal;

        private List<IAnimation> _animations;
        private Character _character;
        private Texture2D _gridTexture;
        private TestBackgroundManager _background;

        private Texture2D _fishTex;

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

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _animations = new List<IAnimation>
            {
                new FishShadowAnimation(new Vector2(EntityConstants.FishShadowLocationXTiles, EntityConstants.FishShadowLocationYTiles))
            };

            foreach (var animation in _animations)
            {
                animation.Load(Content);
            }

            _gridTexture = new Texture2D(GraphicsDevice, 1, 1);
            _gridTexture.SetData(new Color[] { Color.White });

            _background.Load(Content);

            _fishTex = Content.Load<Texture2D>("fish_all");

            _fishDb.LoadContent(Content);

            fishJournal = new FishJournal(_fishDb);

            // Use the character's with and height to center them.  The character is moved up 4 px according to the sprite sheet spec, so subtract an extra 2 tiles from the height to center
            _character = new Character(new Vector2(EntityConstants.CharacterLocationXTiles, EntityConstants.CharacterLocationYTiles));
            _character.Load(Content, _fishDb);

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
            foreach (var animation in _animations)
            {
                animation.Update(gameTime);
            }

            


            _character.Update(gameTime, _background);

            _background.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);
            DrawGrid();

            _background.Draw(_spriteBatch);

            // TODO: Add your drawing code here
            foreach (var animation in _animations)
            {
                animation.Draw(_spriteBatch);
            }

            _character.Draw(_spriteBatch);

            _spriteBatch.Begin();
            FishTexUtils.DrawFish(_spriteBatch, _fishTex, _fishDb, 20, Vector2.Zero);
            _spriteBatch.End();

            base.Draw(gameTime);
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