using FishGame.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace FishGame.Animation.Animations
{
    internal class FishShadowAnimation : IAnimation
    {
        private AnimationGroup _animationGroup;
        private Vector2 _position;

        public FishShadowAnimation(Vector2 position)
        {
            _position = position;
        }

        public void Load(ContentManager content)
        {
            Texture2D fishShadowTexture = content.Load<Texture2D>("fish_shadow");
            _animationGroup = new AnimationGroup(new List<IDrawable> { new SpriteAnimation(fishShadowTexture, 1, 15, 15, EntityConstants.FishShadowWidthTiles, EntityConstants.FishShadowHeightTiles, 0, true) }, 0.2f, _position);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(_animationGroup != null)
            {
                _animationGroup.Draw(spriteBatch);
            }
        }

        public void Reset()
        {
            if( _animationGroup != null )
            {
                _animationGroup.Reset();
            }
        }

        public void Update(GameTime gameTime)
        {
            if( _animationGroup != null )
            {
                _animationGroup.Update(gameTime);
            }
        }
    }
}
