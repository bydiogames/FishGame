using FishGame.Animation;
using FishGame.Animation.Animations;
using FishGame.Backgrounds;
using FishGame.Inventory;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using static FishGame.Animation.AnimationGroup;

namespace FishGame.Entities
{
    internal class Character : IEntity
    {
        private AnimationBase _currentAnimation;
        private AnimationBase _peripheralAnimation;
        private ContentManager _contentManager;
        private CharacterState _state;
        private float _timer;
        private Random _random;
        private FishDB _fishDb;
        private FishJournal _fishJournal;
        private FishRecord _caughtFish;
        private TestBackgroundManager _backgroundManager;
        public Character(Vector2 position, TestBackgroundManager backgroundManager)
        {
            Position = position;
            _currentAnimation = new CharacterIdleAnimation(Position);
            _state = CharacterState.Idle;
            _random = new Random();
            _timer = 0;
            _backgroundManager = backgroundManager;
        }

        public Vector2 Position { get; set; }

        public CharacterState State { get { return _state; } }

        public int UpdateOrder => 0;

        public void Load(ContentManager contentManager, FishDB db, FishJournal fishJournal)
        {
            _currentAnimation.Load(contentManager);
            _contentManager = contentManager;
            _fishDb = db;
            _fishJournal = fishJournal;
        }

        public void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Space) && (_state == CharacterState.Idle || _state == CharacterState.FishIdle || _state == CharacterState.PickupIdle))
            {
                _state = CharacterState.Casting;
                _peripheralAnimation = null;
                _currentAnimation = new CharacterCastAnimation(Position);
                _currentAnimation.Load(_contentManager);
                _currentAnimation.RegisterAnimationFinishedHandler(OnCastCompleted);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Enter) &&  _state == CharacterState.FishOnLine)
            {
                _state = CharacterState.Reeling;
                _peripheralAnimation = null;
                _currentAnimation = new CharacterReelAnimation(Position);
                _currentAnimation.Load(_contentManager);
                _currentAnimation.RegisterAnimationFinishedHandler(OnReelAnimationCompleted);
            }

            _currentAnimation.Update(gameTime);

            // Call fish status update here
            UpdateFishStatus(gameTime);
            if(_peripheralAnimation != null)
            {
                _peripheralAnimation.Update(gameTime);
            }
        }

        private void UpdateFishStatus(GameTime gameTime)
        {
            if (State == CharacterState.FishIdle)
            {
                _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_timer > 2.0f)
                {
                    // 50% chance to catch a fish each elapsed time
                    if (_random.Next(0, 2) > 0)
                    {
                        FishOnLine();
                    }
                    _timer = 0f;
                }
            }
        }
        
        public void Draw(SpriteBatch spriteBatch)
        {
            _currentAnimation.Draw(spriteBatch);
            if(_peripheralAnimation != null)
            {
                _peripheralAnimation.Draw(spriteBatch);
            }
        }

        public void FishOnLine()
        {
            _state = CharacterState.FishOnLine;
            _peripheralAnimation = new ExclamationAnimation(Position);
            _peripheralAnimation.Load(_contentManager);
            Exclamation?.Invoke(this, EventArgs.Empty);
        }

        public void OnCastCompleted(object sender, EventArgs e)
        {
            _state = CharacterState.FishIdle;
            _currentAnimation = new CharacterIdleFishAnimation(Position);
            _currentAnimation.Load(_contentManager);
            CastCompleted?.Invoke(this, EventArgs.Empty);
        }

        public void OnReelAnimationCompleted(object sender, EventArgs e)
        {
            _state = CharacterState.Pickup;
            _currentAnimation = new CharacterPickupAnimation(Position);
            _currentAnimation.Load(_contentManager);
            _currentAnimation.RegisterAnimationFinishedHandler(OnPickupAnimationCompleted);

            // Choose between eligible fish
            IEnumerable<int> eligibleFishIds = _fishDb.GetFishForLocation(_backgroundManager.GetLocation()).Where(id => _fishDb.GetFishById(id).Season.HasFlag(_backgroundManager.GetSeason()));
            int idx = _random.Next(0, eligibleFishIds.Count());
            ref FishRecord caughtFish = ref _fishDb.GetFishById(eligibleFishIds.ElementAt(idx));

            _peripheralAnimation = new CaughtFishPickupAnimation(_fishDb, caughtFish, new Vector2(Position.X, Position.Y + 2));
            _peripheralAnimation.Load(_contentManager);
            _peripheralAnimation.RegisterAnimationStartedHandler(OnPickupAnimationStarted);
            _peripheralAnimation.RegisterAnimationFinishedHandler(OnPickupAnimationFinished);

            ref var invEntry = ref _fishJournal.GetInvSlot(caughtFish.Idx);
            invEntry.HasCollected = true;
            _caughtFish = caughtFish;
            ReelCompleted?.Invoke(this, EventArgs.Empty);
        }

        public void OnPickupAnimationCompleted(object sender, EventArgs e)
        {
            _state = CharacterState.PickupIdle;
            _currentAnimation = new CharacterIdlePickupAnimation(Position);
            _currentAnimation.Load(_contentManager);
            PickupFinished?.Invoke(this, EventArgs.Empty);
        }

        public void OnPickupAnimationStarted(object sender, EventArgs e)
        {
            PickupStarted?.Invoke(this, EventArgs.Empty);
        }

        public void OnPickupAnimationFinished(object sender, EventArgs e)
        {
            PickupFinished?.Invoke(this, EventArgs.Empty);
        }

        public FishRecord GetFish() { return _caughtFish; }

        public event EventHandler PickupStarted;
        public event EventHandler PickupFinished;
        public event EventHandler ReelCompleted;
        public event EventHandler Exclamation;
        public event EventHandler CastCompleted;
    }
}
