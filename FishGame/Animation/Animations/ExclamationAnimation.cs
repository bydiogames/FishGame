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
        public ExclamationAnimation(Vector2 characterPosition) 
        {
            _position = characterPosition;
        }

        public override void Load(ContentManager content)
        {
            Texture2D exclamationTexture = content.Load<Texture2D>("emoticons");
            List<IDrawable> animationList = new List<IDrawable> { new SpriteAnimation(exclamationTexture, 6, 5, 1, EntityConstants.EmoteWidthTiles, EntityConstants.EmoteHeightTiles, 23, shouldLoop: true) };
            _animationGroup = new AnimationGroup(animationList, 0.2f, _position);
            //_animationGroup.AnimationStarted += _onAnimationStart;
        }
    }
}
