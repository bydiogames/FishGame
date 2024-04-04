using CsvHelper.Configuration.Attributes;
using FishGame.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace FishGame.Animation.Animations
{
    internal class ExclamationAnimation : AnimationBase
    {
        private Vector2 _position;
        public ExclamationAnimation(Vector2 characterPosition, OnAnimationCompletion completion) 
        {
            _position = characterPosition;
            _completion = completion;
        }

        public override void Load(ContentManager content)
        {
            Texture2D exclamationTexture = content.Load<Texture2D>("emoticons");
            _animationGroup = new AnimationGroup(new List<IDrawable> { new SpriteAnimation(exclamationTexture, 6, 5, 1, EntityConstants.EmoteWidthTiles, EntityConstants.EmoteHeightTiles, 23, shouldLoop: true) }, 0.2f, _position);
        }
    }
}
