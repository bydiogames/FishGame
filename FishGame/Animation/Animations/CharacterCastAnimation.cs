using FishGame.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace FishGame.Animation.Animations
{
    public class CharacterCastAnimation : IAnimation
    {
        private AnimationGroup _animationGroup;
        private OnAnimationCompletion _completion;
        private Vector2 _position;

        public CharacterCastAnimation(Vector2 position, OnAnimationCompletion completion) 
        {
            _position = position;
            _completion = completion;
        }

        public void Load(ContentManager content)
        {
            Texture2D charTexture = content.Load<Texture2D>("char1_fish");
            Texture2D hairTexture = content.Load<Texture2D>("ponytail_fish");
            Texture2D clothesTexture = content.Load<Texture2D>("dress_fish");

            int destinationWidth = EntityConstants.CharacterWidthTiles * EntityConstants.TileWidthPx;
            int destinationHeight = EntityConstants.CharacterHeightTiles * EntityConstants.TileWidthPx;

            List<SpriteAnimation> sprites = new List<SpriteAnimation> {
                new SpriteAnimation(charTexture, 4, 5, 5, EntityConstants.CharacterWidthTiles, EntityConstants.CharacterHeightTiles, yOffsetPx:16),
                new SpriteAnimation(hairTexture, 4, 70, 5, EntityConstants.CharacterWidthTiles, EntityConstants.CharacterHeightTiles, yOffsetPx:16),
                new SpriteAnimation(clothesTexture, 4, 50, 5, EntityConstants.CharacterWidthTiles, EntityConstants.CharacterHeightTiles, 30, yOffsetPx:16)
            };

            _animationGroup = new AnimationGroup(sprites, 0.2f);
        }

        public void Reset()
        {
            if(_animationGroup != null)
            {
                _animationGroup.Reset();
            }
        }

        public void Update(GameTime gameTime)
        {
            if(_animationGroup != null)
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
            if(_animationGroup != null)
            {
                _animationGroup.Draw(spriteBatch, _position);
            }
        }
    }
}
