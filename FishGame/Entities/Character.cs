using FishGame.Animation;
using FishGame.Animation.Animations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FishGame.Entities
{
    internal class Character : IEntity
    {
        private IAnimation _currentAnimation;
        private ContentManager _contentManager;
        private CharacterState _state;
        public Character(Vector2 position)
        {
            Position = position;
            _currentAnimation = new CharacterIdleAnimation(Position);
            _state = CharacterState.Idle;
        }

        public Vector2 Position { get; set; }

        public CharacterState State { get { return _state; } }

        public void Load(ContentManager contentManager)
        {
            _currentAnimation.Load(contentManager);
            _contentManager = contentManager;
        }

        public void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Space) && (_state == CharacterState.Idle || _state == CharacterState.FishIdle || _state == CharacterState.PickupIdle))
            {
                _state = CharacterState.Casting;
                _currentAnimation = new CharacterCastAnimation(Position, OnCastAnimationCompletion);
                _currentAnimation.Load(_contentManager);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Enter) && _state == CharacterState.FishIdle)
            {
                _state = CharacterState.Reeling;
                _currentAnimation = new CharacterReelAnimation(Position, OnReelAnimationCompletion);
                _currentAnimation.Load(_contentManager);
            }

            _currentAnimation.Update(gameTime);
        }
        
        public void Draw(SpriteBatch spriteBatch)
        {
            _currentAnimation.Draw(spriteBatch);
        }

        public void OnCastAnimationCompletion()
        {
            _state = CharacterState.FishIdle;
            _currentAnimation = new CharacterIdleFishAnimation(Position);
            _currentAnimation.Load(_contentManager);
        }

        public void OnReelAnimationCompletion()
        {
            _state = CharacterState.Pickup;
            _currentAnimation = new CharacterPickupAnimation(Position, OnPickupAnimationCompletion);
            _currentAnimation.Load(_contentManager);
        }

        public void OnPickupAnimationCompletion()
        {
            _state = CharacterState.PickupIdle;
            _currentAnimation = new CharacterIdlePickupAnimation(Position);
            _currentAnimation.Load(_contentManager);
        }
    }
}
