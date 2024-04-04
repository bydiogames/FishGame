using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using FishGame.Entities;

namespace FishGame.Animation.Animations
{
    internal class CharacterIdleAnimation : AnimationBase
    {
        private Vector2 _position;

        public CharacterIdleAnimation(Vector2 position)
        {
            _position = position;
        }

        public override void Load(ContentManager content)
        {
            Texture2D charTexture = content.Load<Texture2D>("char1_pickup");
            Texture2D hairTexture = content.Load<Texture2D>("ponytail_pickup");
            Texture2D clothesTexture = content.Load<Texture2D>("dress_pickup");

            List<IDrawable> sprites = new List<IDrawable> {
                new SpriteAnimation(charTexture, 4, 5, 1, EntityConstants.CharacterWidthTiles, EntityConstants.CharacterHeightTiles, xOffsetPx:EntityConstants.CharacterXOffsetPx),
                new SpriteAnimation(hairTexture, 4, 70, 1, EntityConstants.CharacterWidthTiles, EntityConstants.CharacterHeightTiles, xOffsetPx:EntityConstants.CharacterXOffsetPx),
                new SpriteAnimation(clothesTexture, 4, 50, 1, EntityConstants.CharacterWidthTiles, EntityConstants.CharacterHeightTiles, 30, xOffsetPx:EntityConstants.CharacterXOffsetPx)
            };

            _animationGroup = new AnimationGroup(sprites, 0.2f, _position);
        }
    }
}
