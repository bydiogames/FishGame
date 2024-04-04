using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace FishGame.Animation
{
    public delegate void OnAnimationCompletion();
    
    internal abstract class AnimationBase
    {
        protected AnimationGroup _animationGroup;
        protected OnAnimationCompletion _completion;

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
                if (_animationGroup.IsFinished() && _completion != null)
                {
                    _completion.Invoke();
                }
            }
        }
    }
}
