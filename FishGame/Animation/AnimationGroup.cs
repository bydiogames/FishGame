using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FishGame.Animation
{
    internal class AnimationGroup
    {
        private float _timer;
        private float _frameSpeed;
        private List<IDrawable> _sprites;
        private PositionManager _positionManager;
        private int _delayFrames;
        private int _currentFrame = 1;
        public AnimationGroup(List<IDrawable> sprites, float frameSpeed, Vector2 position, PositionManager positionManager = null, int delayFrames = 0)
        {
            _sprites = sprites;
            _frameSpeed = frameSpeed;
            _positionManager = positionManager != null ? positionManager : new PositionManager(position, 1);
            _delayFrames = delayFrames;
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
            if(_sprites.All(s => s.IsFinished()))
            {
                AnimationFinished?.Invoke(this, EventArgs.Empty);
                return;
            }

            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_timer > _frameSpeed)
            {
                // Notify that the animation has started playing for syncing sounds and other effects
                if (_currentFrame == 1 + _delayFrames) { AnimationStarted?.Invoke(this, EventArgs.Empty); }

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

        public event EventHandler AnimationStarted;
        public event EventHandler AnimationFinished;

        public delegate void AnimationFinishedEventHandler(object sender, EventArgs e);
        public delegate void AnimationStartedEventHandler(object sender, EventArgs e);

    }
}
