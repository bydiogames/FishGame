using FishGame.Entities;
using FishGame.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace FishGame.Interface
{
    internal class BydiogameUI : Screen, IGameComponent
    {
        private readonly CoroutineManager _coroutineManager;
        public event Action RequestGotoLocationScreen;

        public BydiogameUI(Game1 game, GraphicsDevice device, ContentManager content, CoroutineManager coroutineManager) : base(game, device, content) 
        {
            _coroutineManager = coroutineManager;
        }
        
        private Texture2D _tex;
        private Animation.SpriteAnimation _anim;

        private SpriteFont _font;

        public override void LoadContent()
        {
            _tex = _content.Load<Texture2D>("bydiogames_logo");
            _font = _content.Load<SpriteFont>("logofont");

            _anim = new Animation.SpriteAnimation(_tex, 10, 4, 40, 800 / EntityConstants.TileWidthPx, 480 / EntityConstants.TileHeightPx, shouldLoop: true);

            _coroutineManager.Start(StartupRoutine());
        }

        private string _drawingText = "";

        private IEnumerator<IWaitable> StartupRoutine()
        {
            yield return new Wait(TimeSpan.FromSeconds(1));
            _drawingText = "By Bydiogame Studios";

            yield return new Wait(TimeSpan.FromSeconds(3));
            _game.LoadMainUI();
        }

        private bool visible = true;

        public bool Visible
        {
            get => visible; set
            {
                visible = value;
                if (VisibleChanged != null)
                    VisibleChanged(this, EventArgs.Empty);
            }
        }

        public event EventHandler<EventArgs> DrawOrderChanged;

        public event EventHandler<EventArgs> VisibleChanged;

        private TimeSpan _frameTime;

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _frameTime += gameTime.ElapsedGameTime;
            if (_frameTime.TotalSeconds > (1f / 30f))
            {
                _anim.Update();
                _frameTime = TimeSpan.Zero;
            }

            _anim.Draw(spriteBatch, Vector2.Zero);
            var textDim = _font.MeasureString(_drawingText);
            var location = spriteBatch.GraphicsDevice.Viewport.Bounds.Center.ToVector2() - (textDim * 0.5f) - new Vector2(0, 100);
            spriteBatch.DrawString(_font, _drawingText, location, Color.White);
        }

        void IGameComponent.Initialize()
        {
        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}
