using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace FishGame.Animation
{
    internal abstract class AnimationBase
    {
        protected AnimationGroup _animationGroup;

        public abstract void Load(ContentManager content);

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

        public void RegisterAnimationStartedHandler(EventHandler handler)
        {
            if( _animationGroup != null )
            {
                _animationGroup.AnimationStarted += handler;
            }
        }

        public void RegisterAnimationFinishedHandler(EventHandler handler)
        {
            if (_animationGroup != null)
            {
                _animationGroup.AnimationFinished += handler;
            }
        }
    }
}
