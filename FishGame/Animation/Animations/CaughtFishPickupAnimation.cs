using FishGame.Entities;
using FishGame.Inventory;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;

namespace FishGame.Animation.Animations
{
    internal class CaughtFishPickupAnimation : AnimationBase
    {
        private Rectangle _sourceRectangle;
        private Vector2 _position;

        public CaughtFishPickupAnimation(FishDB db, FishRecord fish, Vector2 position)
        {
            _position = position;
            _sourceRectangle = FishTexUtils.GetFishTilePxRect(db, fish.Idx);
        }

        public override void Load(ContentManager content)
        {
            Texture2D fishTexture = content.Load<Texture2D>("fish_all");
            List<IDrawable> sprites = new List<IDrawable> { new Sprite(fishTexture, _sourceRectangle, EntityConstants.CaughtFishWidthTiles, EntityConstants.CaughtFishHeightTiles, xOffsetPx:16) };
            _animationGroup = new AnimationGroup(sprites, 0.2f, _position, new PositionManager(_position, 4, 0, -0.25f), delayFrames:2);
        }
    }
}
