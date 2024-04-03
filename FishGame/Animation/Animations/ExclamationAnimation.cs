using CsvHelper.Configuration.Attributes;
using FishGame.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace FishGame.Animation.Animations
{
    internal class ExclamationAnimation : IAnimation
    {
        private Vector2 _position;
        private OnAnimationCompletion _completion;
        private AnimationGroup _animationGroup;
        public ExclamationAnimation(Vector2 characterPosition, OnAnimationCompletion completion) 
        {
            _position = characterPosition;
            _completion = completion;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            _animationGroup.Draw(spriteBatch);
        }

        public void Load(ContentManager content)
        {
            Texture2D exclamationTexture = content.Load<Texture2D>("emoticons");
            _animationGroup = new AnimationGroup(new List<IDrawable> { new SpriteAnimation(exclamationTexture, 6, 5, 1, EntityConstants.EmoteWidthTiles, EntityConstants.EmoteHeightTiles, 23, shouldLoop:true) }, 0.2f, _position);
        }

        public void Reset()
        {
            _animationGroup.Reset();
        }

        public void Update(GameTime gameTime)
        {
            _animationGroup.Update(gameTime);
            if(_animationGroup.IsFinished())
            {
                _completion.Invoke();
            }
        }
    }
}
