using FishGame.Animation;
using FishGame.Animation.Animations;
using FishGame.Entities;
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
        private List<IAnimation> _animations;
        private Character _character;
        private Texture2D _tileTextures;
        private Texture2D _gridTexture;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Use the character's with and height to center them.  The character is moved up 4 px according to the sprite sheet spec, so subtract an extra 2 tiles from the height to center
            _character = new Character(new Vector2((EntityConstants.ScreenWidthTiles / 2) - (EntityConstants.CharacterWidthTiles / 2), (EntityConstants.ScreenHeightTiles / 2) - (EntityConstants.CharacterHeightTiles / 2) - 2));
            _character.Load(Content);

            _animations = new List<IAnimation>
            {
                new FishShadowAnimation(new Vector2((EntityConstants.ScreenWidthTiles / 2) - 1, (EntityConstants.ScreenHeightTiles / 2) + 6))
            };

            foreach (var animation in _animations)
            {
                animation.Load(Content);
            }

            // Manually load backgrounds for now so we can figure out layering
            _tileTextures = Content.Load<Texture2D>("tiles_all");

            _gridTexture = new Texture2D(GraphicsDevice, 1, 1);
            _gridTexture.SetData(new Color[] { Color.White });

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

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);
            DrawGrid();

            // Drawing the dock
            int tileWidth = _tileTextures.Width / 27;
            int tileHeight = _tileTextures.Height / 27;
            int xLocation = (EntityConstants.ScreenWidthTiles / 2) - (2 * (EntityConstants.DockWidthTiles / 3));
            int yLocation = (EntityConstants.ScreenHeightTiles / 2) - (3 * (EntityConstants.DockHeightTiles) / 4) + 1;

            Rectangle sourceRectangle = new Rectangle(tileWidth * 9, tileHeight * 15, 3 * tileWidth, 4 * tileHeight);
            Rectangle destinationRectangle = new Rectangle(xLocation * EntityConstants.TileWidthPx,
                yLocation * EntityConstants.TileHeightPx,
                EntityConstants.DockWidthTiles * EntityConstants.TileWidthPx, /*width*/
                EntityConstants.DockHeightTiles * EntityConstants.TileHeightPx /*height*/);

            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
            _spriteBatch.Draw(_tileTextures, destinationRectangle, sourceRectangle, Color.White);
            _spriteBatch.End();

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