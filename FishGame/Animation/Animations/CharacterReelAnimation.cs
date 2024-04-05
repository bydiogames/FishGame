using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FishGame.Entities;

namespace FishGame.Animation.Animations
{
    internal class CharacterReelAnimation : AnimationBase
    {
        private Vector2 _position;

        public CharacterReelAnimation(Vector2 position)
        {
            _position = position;
        }

        public override void Load(ContentManager content)
        {
            Texture2D charTexture = content.Load<Texture2D>("char1_fish");
            Texture2D hairTexture = content.Load<Texture2D>("ponytail_fish");
            Texture2D clothesTexture = content.Load<Texture2D>("dress_fish");

            List<IDrawable> sprites = new List<IDrawable> {
                new SpriteAnimation(charTexture, 4, 5, 3, EntityConstants.CharacterWidthTiles, EntityConstants.CharacterHeightTiles, xOffsetPx:EntityConstants.CharacterXOffsetPx, yOffsetPx:EntityConstants.CharacterFishingYOffsetPx),
                new SpriteAnimation(hairTexture, 4, 70, 3, EntityConstants.CharacterWidthTiles, EntityConstants.CharacterHeightTiles, xOffsetPx:EntityConstants.CharacterXOffsetPx, yOffsetPx:EntityConstants.CharacterFishingYOffsetPx),
                new SpriteAnimation(clothesTexture, 4, 50, 3, EntityConstants.CharacterWidthTiles, EntityConstants.CharacterHeightTiles, 30, xOffsetPx:EntityConstants.CharacterXOffsetPx, yOffsetPx:EntityConstants.CharacterFishingYOffsetPx)
            };

            _animationGroup = new AnimationGroup(sprites, 0.2f, _position);
        }
    }
}

