﻿using FishGame.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace FishGame.Animation.Animations
{
    internal class CharacterCastAnimation : AnimationBase
    {
        private Vector2 _position;

        public CharacterCastAnimation(Vector2 position, OnAnimationCompletion completion) 
        {
            _position = position;
            _completion = completion;
        }

        public override void Load(ContentManager content)
        {
            Texture2D charTexture = content.Load<Texture2D>("char1_fish");
            Texture2D hairTexture = content.Load<Texture2D>("ponytail_fish");
            Texture2D clothesTexture = content.Load<Texture2D>("dress_fish");

            int destinationWidth = EntityConstants.CharacterWidthTiles * EntityConstants.TileWidthPx;
            int destinationHeight = EntityConstants.CharacterHeightTiles * EntityConstants.TileWidthPx;

            List<IDrawable> sprites = new List<IDrawable> {
                new SpriteAnimation(charTexture, 4, 5, 5, EntityConstants.CharacterWidthTiles, EntityConstants.CharacterHeightTiles, xOffsetPx:EntityConstants.CharacterXOffsetPx, yOffsetPx:EntityConstants.CharacterFishingYOffsetPx),
                new SpriteAnimation(hairTexture, 4, 70, 5, EntityConstants.CharacterWidthTiles, EntityConstants.CharacterHeightTiles, xOffsetPx:EntityConstants.CharacterXOffsetPx, yOffsetPx:EntityConstants.CharacterFishingYOffsetPx),
                new SpriteAnimation(clothesTexture, 4, 50, 5, EntityConstants.CharacterWidthTiles, EntityConstants.CharacterHeightTiles, 30, xOffsetPx:EntityConstants.CharacterXOffsetPx, yOffsetPx:EntityConstants.CharacterFishingYOffsetPx)
            };

            _animationGroup = new AnimationGroup(sprites, 0.2f, _position);
        }
    }
}
