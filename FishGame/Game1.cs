using FishGame.Animation;
using FishGame.Animation.Animations;
using FishGame.Backgrounds;
using FishGame.Entities;
using FishGame.Inventory;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace FishGame
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private FishDB _fishDb;
        private List<IAnimation> _animations;
        private Character _character;
        private Texture2D _gridTexture;
        private TestBackgroundManager _background;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _background = new TestBackgroundManager(Season.Spring, Location.Pond);

            _fishDb = new FishDB();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Use the character's with and height to center them.  The character is moved up 4 px according to the sprite sheet spec, so subtract an extra 2 tiles from the height to center
            _character = new Character(new Vector2(EntityConstants.CharacterLocationXTiles, EntityConstants.CharacterLocationYTiles));
            _character.Load(Content);

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

            _fishDb.LoadContent(Content);
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

            _character.Update(gameTime);

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