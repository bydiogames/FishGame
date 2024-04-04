using FishGame.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace FishGame.Animation.Animations
{
    internal class FishShadowAnimation : AnimationBase
    {
        private Vector2 _position;

        public FishShadowAnimation(Vector2 position)
        {
            _position = position;
        }

        public override void Load(ContentManager content)
        {
            Texture2D fishShadowTexture = content.Load<Texture2D>("fish_shadow");
            _animationGroup = new AnimationGroup(new List<IDrawable> { new SpriteAnimation(fishShadowTexture, 1, 15, 15, EntityConstants.FishShadowWidthTiles, EntityConstants.FishShadowHeightTiles, 0, true) }, 0.2f, _position);
        }
    }
}
