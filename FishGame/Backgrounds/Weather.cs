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

        private Texture2D[] _weatherTexs = new Texture2D[SeasonUtils.Count];
        private SpriteAnimation[] _weatherAnims = new SpriteAnimation[SeasonUtils.Count];

        private static readonly string[] SeasonWeatherAssetName = new string[]
        {
            "rain",
            "fairys",
            "wind",
            "snow"
        };

        private Texture2D _snowTex;
        private SpriteAnimation _snowSpriteAnim;

        public Weather(TestBackgroundManager backgroundManager, SpriteBatch spriteBatch)
        {
            this.backgroundManager = backgroundManager;
            this.spriteBatch = spriteBatch;
        }

        public void Load(ContentManager content)
        {
            for(int i = 0; i < SeasonUtils.Count; ++i)
            {
                if (SeasonWeatherAssetName[i] == null)
                    continue;

                Texture2D tex = _weatherTexs[i] = content.Load<Texture2D>(SeasonWeatherAssetName[i]);
                int rows = tex.Height / 208;
                int cols = tex.Width / 160;
                _weatherAnims[i] = new SpriteAnimation(tex, rows, cols, rows * cols, 160 / EntityConstants.TileWidthPx * 2, 208 / EntityConstants.TileHeightPx * 2, shouldLoop: true);
            }
        }

        int Microsoft.Xna.Framework.IDrawable.DrawOrder => 1;

        bool Microsoft.Xna.Framework.IDrawable.Visible => false;

        public event EventHandler<EventArgs> DrawOrderChanged;

        public event EventHandler<EventArgs> VisibleChanged;

        private TimeSpan _frameTimer;

        void Microsoft.Xna.Framework.IDrawable.Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            _frameTimer += gameTime.ElapsedGameTime;

            int seasonIdx = System.Numerics.BitOperations.Log2((uint)backgroundManager.GetSeason());
            if (_weatherAnims[seasonIdx] != null)
            {
                if (_frameTimer.TotalSeconds > (1f / 30f))
                {
                    _frameTimer = TimeSpan.Zero;
                    _weatherAnims[seasonIdx].Update();
                }

                _weatherAnims[seasonIdx].Draw(spriteBatch, new Vector2(2));
            }

            spriteBatch.End();
        }

        void IGameComponent.Initialize()
        {
        }
    }
}
