using FishGame.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace FishGame.Animation.Animations
{
    internal class CharacterIdleFishAnimation : AnimationBase
    {
        private Vector2 _position;

        public CharacterIdleFishAnimation(Vector2 position)
        {
            _position = position;
        }

        public override void Load(ContentManager content)
        {
            Texture2D charTexture = content.Load<Texture2D>("char1_fish");
            Texture2D hairTexture = content.Load<Texture2D>("ponytail_fish");
            Texture2D clothesTexture = content.Load<Texture2D>("dress_fish");

            List<IDrawable> sprites = new List<IDrawable> {
                new SpriteAnimation(charTexture, 4, 5, 1, EntityConstants.CharacterWidthTiles, EntityConstants.CharacterHeightTiles, xOffsetPx:EntityConstants.CharacterXOffsetPx, yOffsetPx:EntityConstants.CharacterFishingYOffsetPx),
                new SpriteAnimation(hairTexture, 4, 70, 1, EntityConstants.CharacterWidthTiles, EntityConstants.CharacterHeightTiles, xOffsetPx:EntityConstants.CharacterXOffsetPx, yOffsetPx:EntityConstants.CharacterFishingYOffsetPx),
                new SpriteAnimation(clothesTexture, 4, 50, 1, EntityConstants.CharacterWidthTiles, EntityConstants.CharacterHeightTiles, 30, xOffsetPx:EntityConstants.CharacterXOffsetPx, yOffsetPx:EntityConstants.CharacterFishingYOffsetPx)
            };

            _animationGroup = new AnimationGroup(sprites, 0.2f, _position);
        }
    }
}
