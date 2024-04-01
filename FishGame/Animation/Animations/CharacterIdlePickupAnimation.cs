using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using FishGame.Entities;

namespace FishGame.Animation.Animations
{
    internal class CharacterIdlePickupAnimation : IAnimation
    {
        private AnimationGroup _animationGroup;
        private Vector2 _position;

        public CharacterIdlePickupAnimation(Vector2 position)
        {
            _position = position;
        }

        public void Load(ContentManager content)
        {
            Texture2D charTexture = content.Load<Texture2D>("char1_pickup");
            Texture2D hairTexture = content.Load<Texture2D>("ponytail_pickup");
            Texture2D clothesTexture = content.Load<Texture2D>("dress_pickup");

            List<SpriteAnimation> sprites = new List<SpriteAnimation> {
                new SpriteAnimation(charTexture, 4, 5, 5, EntityConstants.CharacterWidthTiles, EntityConstants.CharacterHeightTiles),
                new SpriteAnimation(hairTexture, 4, 70, 5, EntityConstants.CharacterWidthTiles, EntityConstants.CharacterHeightTiles),
                new SpriteAnimation(clothesTexture, 4, 50, 5, EntityConstants.CharacterWidthTiles, EntityConstants.CharacterHeightTiles, startingFrame : 30)
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
