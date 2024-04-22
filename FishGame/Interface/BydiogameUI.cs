using FishGame.Entities;
using FishGame.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace FishGame.Interface
{
    internal class BydiogameUI : IGameComponent, IDrawable
    {
        private SpriteBatch _spriteBatch;
        private readonly CoroutineManager coroutineManager;

        public event Action RequestGotoLocationScreen;

        public BydiogameUI(SpriteBatch spriteBatch, CoroutineManager coroutineManager)
        {
            _spriteBatch = spriteBatch;
            this.coroutineManager = coroutineManager;
        }

        private Texture2D _tex;
        private Animation.SpriteAnimation _anim;

        private SpriteFont _font;

        public void Load(ContentManager content)
        {
            _tex = content.Load<Texture2D>("bydiogames_logo");
            _font = content.Load<SpriteFont>("logofont");

            _anim = new Animation.SpriteAnimation(_tex, 10, 4, 40, 800 / EntityConstants.TileWidthPx, 480 / EntityConstants.TileHeightPx, shouldLoop: true);

            this.coroutineManager.Start(StartupRoutine());
        }

        private string _drawingText = "";

        private IEnumerator<IWaitable> StartupRoutine()
        {
            yield return new Wait(TimeSpan.FromSeconds(1));
            _drawingText = "By Bydiogame Studios";

            yield return new Wait(TimeSpan.FromSeconds(3));
            Visible = false;
        }

        int IDrawable.DrawOrder => 50;

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

        void IDrawable.Draw(GameTime gameTime)
        {
            _frameTime += gameTime.ElapsedGameTime;
            if (_frameTime.TotalSeconds > (1f / 30f))
            {
                _anim.Update();
                _frameTime = TimeSpan.Zero;
            }

            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
            _anim.Draw(_spriteBatch, Vector2.Zero);
            var textDim = _font.MeasureString(_drawingText);
            var location = _spriteBatch.GraphicsDevice.Viewport.Bounds.Center.ToVector2() - (textDim * 0.5f) - new Vector2(0, 100);
            _spriteBatch.DrawString(_font, _drawingText, location, Color.White);
            _spriteBatch.End();
        }

        void IGameComponent.Initialize()
        {
        }
    }
}
