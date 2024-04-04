using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace FishGame.Animation
{
    public delegate void OnAnimationStart();
    internal class AnimationGroup
    {
        private float _timer;
        private float _frameSpeed;
        private List<IDrawable> _sprites;
        private PositionManager _positionManager;
        private int _delayFrames;
        private int _currentFrame = 1;
        private OnAnimationStart _onStart;
        public AnimationGroup(List<IDrawable> sprites, float frameSpeed, Vector2 position, PositionManager positionManager = null, int delayFrames = 0, OnAnimationStart onStart = null)
        {
            _sprites = sprites;
            _frameSpeed = frameSpeed;
            _positionManager = positionManager != null ? positionManager : new PositionManager(position, 1);
            _delayFrames = delayFrames;
            _onStart = onStart;
        }

        public void Reset()
        {
            _timer = 0;
            _currentFrame = 1;
            foreach (var sprite in _sprites)
            {
                sprite.Reset();
            }
        }

        public void Update(GameTime gameTime)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_timer > _frameSpeed)
            {
                // Notify that the animation has started playing for syncing sounds and other effects
                if (_onStart != null && _currentFrame == 1 + _delayFrames) { _onStart.Invoke(); }

                _currentFrame++;
                _timer = 0;
                if(_currentFrame > _delayFrames)
                {
                    foreach (IDrawable sprite in _sprites)
                    {
                        sprite.Update();
                        _positionManager.Update();
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(_currentFrame > _delayFrames)
            {
                foreach (IDrawable sprite in _sprites)
                {
                    sprite.Draw(spriteBatch, _positionManager.GetCurrentPosition());
                }
            }
        }

        public bool IsFinished()
        {
            return _sprites.All(s => s.IsFinished());
        }
    }
}
