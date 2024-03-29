using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace FishGame.Animation
{
    internal class AnimationGroup
    {
        private float _timer;
        private float _frameSpeed;
        private List<SpriteAnimation> _sprites;

        public AnimationGroup(List<SpriteAnimation> sprites, float frameSpeed)
        {
            _sprites = sprites;
            _frameSpeed = frameSpeed;
        }

        public void Reset()
        {
            _timer = 0;
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
                _timer = 0;
                foreach (SpriteAnimation sprite in _sprites)
                {
                    sprite.Update();
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            foreach (SpriteAnimation sprite in _sprites)
            {
                sprite.Draw(spriteBatch, position);
            }
        }

        public bool IsFinished()
        {
            return _sprites.All(s => s.IsFinished());
        }
    }
}
