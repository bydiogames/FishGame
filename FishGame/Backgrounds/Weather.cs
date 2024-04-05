using FishGame.Animation;
using FishGame.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishGame.Backgrounds
{
    internal class Weather : IGameComponent, Microsoft.Xna.Framework.IDrawable
    {
        private readonly TestBackgroundManager backgroundManager;
        private readonly SpriteBatch spriteBatch;
        private Texture2D _snowTex;
        private SpriteAnimation _snowSpriteAnim;

        public Weather(TestBackgroundManager backgroundManager, SpriteBatch spriteBatch)
        {
            this.backgroundManager = backgroundManager;
            this.spriteBatch = spriteBatch;
        }

        public void Load(ContentManager content)
        {
            _snowTex = content.Load<Texture2D>("snow");
            int rows = _snowTex.Height / 208;
            int cols = _snowTex.Width / 160;
            _snowSpriteAnim = new SpriteAnimation(_snowTex, rows, cols, 800, 160 / EntityConstants.TileWidthPx * 2, 208 / EntityConstants.TileHeightPx * 2, shouldLoop: true );
        }

        int Microsoft.Xna.Framework.IDrawable.DrawOrder => 1;

        bool Microsoft.Xna.Framework.IDrawable.Visible => true;

        public event EventHandler<EventArgs> DrawOrderChanged;

        public event EventHandler<EventArgs> VisibleChanged;

        private TimeSpan _frameTimer;

        void Microsoft.Xna.Framework.IDrawable.Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            if (backgroundManager.GetSeason().HasFlag(Entities.Season.Winter))
            {
                _frameTimer += gameTime.ElapsedGameTime;

                if (_frameTimer.TotalSeconds > (1f / 30f))
                {
                    _snowSpriteAnim.Update();
                    _frameTimer = TimeSpan.Zero;
                }

                _snowSpriteAnim.Draw(spriteBatch, new Vector2(2));
            }

            spriteBatch.End();
        }

        void IGameComponent.Initialize()
        {
        }
    }
}
