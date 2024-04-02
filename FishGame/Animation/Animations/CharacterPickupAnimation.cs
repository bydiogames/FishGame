using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using FishGame.Entities;

namespace FishGame.Animation.Animations
{
    internal class CharacterPickupAnimation : IAnimation
    {
        private AnimationGroup _animationGroup;
        private OnAnimationCompletion _completion;
        private Vector2 _position;

        public CharacterPickupAnimation(Vector2 position, OnAnimationCompletion completion)
        {
            _position = position;
            _completion = completion;
        }

        public void Load(ContentManager content)
        {
            Texture2D charTexture = content.Load<Texture2D>("char1_pickup");
            Texture2D hairTexture = content.Load<Texture2D>("ponytail_pickup");
            Texture2D clothesTexture = content.Load<Texture2D>("dress_pickup");

            List<SpriteAnimation> sprites = new List<SpriteAnimation> {
                new SpriteAnimation(charTexture, 4, 5, 1, EntityConstants.CharacterWidthTiles, EntityConstants.CharacterHeightTiles, startingFrame:4, xOffsetPx : EntityConstants.CharacterXOffsetPx),
                new SpriteAnimation(hairTexture, 4, 70, 1, EntityConstants.CharacterWidthTiles, EntityConstants.CharacterHeightTiles, startingFrame:4, xOffsetPx : EntityConstants.CharacterXOffsetPx),
                new SpriteAnimation(clothesTexture, 4, 50, 1, EntityConstants.CharacterWidthTiles, EntityConstants.CharacterHeightTiles, startingFrame:34, xOffsetPx:EntityConstants.CharacterXOffsetPx)
            };

            _animationGroup = new AnimationGroup(sprites, 0.2f);
        }

        public void Reset()
        {
            if (_animationGroup != null)
            {
                _animationGroup.Reset();
            }
        }

        public void Update(GameTime gameTime)
        {
            if (_animationGroup != null)
            {
                if (_animationGroup.IsFinished())
                {
                    _completion.Invoke();
                    return;
                }

                _animationGroup.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_animationGroup != null)
            {
                _animationGroup.Draw(spriteBatch, _position);
            }
        }
    }
}
