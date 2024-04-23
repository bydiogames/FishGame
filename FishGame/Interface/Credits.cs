using FishGame.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;

namespace FishGame.Interface
{
    internal class Credits : Screen, IGameComponent
    {
        private static readonly int ScrollSpeedPxPerSec = 60;

        private SpriteBatch _spriteBatch;
        private readonly CoroutineManager coroutineManager;

        public event Action RequestGotoLocationScreen;

        public Credits(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, SpriteBatch spriteBatch, CoroutineManager coroutineManager) : base(game, graphicsDevice, content)
        {
            _spriteBatch = spriteBatch;
            this.coroutineManager = coroutineManager;
        }

        private Texture2D _tex;
        private Animation.SpriteAnimation _anim;

        private SpriteFont _font;

        private List<string> _credits = new List<string>();
        private float _creditScroll;
        private float _creditsHeight;

        public override void LoadContent()
        {
            _tex = _content.Load<Texture2D>("bydiogames_logo");
            _font = _content.Load<SpriteFont>("logofont");

            using var creditsFileStream = File.OpenRead($"{_content.RootDirectory}/credits.txt");
            using TextReader textReader = new StreamReader(creditsFileStream);

            for(string credit = textReader.ReadLine(); credit != null; credit = textReader.ReadLine())
            {
                int result;
                if(Int32.TryParse(credit, System.Globalization.NumberStyles.Number, null, out result))
                {
                    for (int i = 0; i < result; i++)
                    {
                        _credits.Insert(0, " ");
                        _creditsHeight += _font.MeasureString(credit).Y;
                    }
                }
                else
                {
                    _credits.Insert(0, credit);
                    _creditsHeight += _font.MeasureString(credit).Y;
                }
            }

        }

        private IEnumerator<IWaitable> StartupRoutine()
        {
            // How many pixels to scroll.
            var scrollDistance = _creditsHeight + _spriteBatch.GraphicsDevice.Viewport.Height;

            var startTime = DateTime.UtcNow;

            while(_creditScroll < scrollDistance)
            {
                var timePassed = (DateTime.UtcNow - startTime).TotalSeconds;
                _creditScroll = (int)(ScrollSpeedPxPerSec * timePassed) - 80;

                yield return null;
            }

            _game.LoadMainUI();
        }

        private bool visible = true;

        public bool Visible
        {
            get => visible; set
            {
                bool didChange = visible != value;
                visible = value;
                if (VisibleChanged != null && didChange)
                    VisibleChanged(this, EventArgs.Empty);

                if (visible && didChange)
                {
                    this.coroutineManager.Start(StartupRoutine());
                }
            }
        }

        public event EventHandler<EventArgs> DrawOrderChanged;

        public event EventHandler<EventArgs> VisibleChanged;

        private TimeSpan _frameTime;

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var posY = _creditScroll;
            foreach(var credit in _credits)
            {
                var creditDim = _font.MeasureString(credit).ToPoint();
                var creditPos = new Point(spriteBatch.GraphicsDevice.Viewport.Bounds.Center.X, 0) - new Point(creditDim.X / 2, 0) + new Point(0, (int)posY);

                var creditRect = new Rectangle(creditPos, creditDim);
                if (spriteBatch.GraphicsDevice.Viewport.Bounds.Intersects(creditRect))
                {
                    spriteBatch.DrawString(_font, credit, creditPos.ToVector2(), Color.White);
                }

                posY -= creditDim.Y;
            }

            spriteBatch.End();
        }

        void IGameComponent.Initialize()
        {
        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}
