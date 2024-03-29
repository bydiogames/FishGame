using FishGame.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace FishGame.Animation.Animations
{
    internal class ExclamationAnimation : IAnimation
    {
        private Character _character;
        private AnimationGroup _animationGroup;
        public ExclamationAnimation(Character character) 
        {
        
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 position = new Vector2(_character.Position.X + 20, _character.Position.Y + 20);
            _animationGroup.Draw(spriteBatch, position);
        }

        public void Load(ContentManager content)
        {
            Texture2D exclamationTexture = content.Load<Texture2D>("exclamation");
            _animationGroup = new AnimationGroup(new List<SpriteAnimation> { new SpriteAnimation(exclamationTexture, 1, 1, 1, 64, 64, 0) }, 0.2f);
        }

        public void Reset()
        {
            _animationGroup.Reset();
        }

        public void Update(GameTime gameTime)
        {
            _animationGroup.Update(gameTime);
        }
    }
}
