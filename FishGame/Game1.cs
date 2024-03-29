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

            _character = new Character(new Vector2(320, 120));
            _character.Load(Content);

            _animations = new List<IAnimation>
            {
                new FishShadowAnimation(new Vector2(370, 300))
            };

            foreach (var animation in _animations)
            {
                animation.Load(Content);
            }
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
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            foreach (var animation in _animations)
            {
                animation.Draw(_spriteBatch);
            }

            _character.Draw(_spriteBatch);

            base.Draw(gameTime);
        }
    }
}