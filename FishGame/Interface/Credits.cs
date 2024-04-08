using FishGame.Backgrounds;
using FishGame.Entities;
using FishGame.Inventory;
using FishGame.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FishGame.Interface
{
    internal class Credits : IGameComponent, IDrawable
    {
        private static readonly int ScrollSpeedPxPerSec = 60;

        private SpriteBatch _spriteBatch;
        private readonly CoroutineManager coroutineManager;

        public event Action RequestGotoLocationScreen;

        public Credits(SpriteBatch spriteBatch, CoroutineManager coroutineManager)
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

        public void Load(ContentManager content)
        {
            _tex = content.Load<Texture2D>("bydiogames_logo");
            _font = content.Load<SpriteFont>("logofont");

            using var creditsFileStream = File.OpenRead($"{content.RootDirectory}/credits.txt");
            using TextReader textReader = new StreamReader(creditsFileStream);

            for(string credit = textReader.ReadLine(); credit != null; credit = textReader.ReadLine())
            {
                _credits.Insert(0, credit);
                _creditsHeight += _font.MeasureString(credit).Y;
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

            Visible = false;
        }

        int IDrawable.DrawOrder => 50;

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

        void IDrawable.Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);

            var posY = _creditScroll;
            foreach(var credit in _credits)
            {
                var creditDim = _font.MeasureString(credit).ToPoint();
                var creditPos = new Point(_spriteBatch.GraphicsDevice.Viewport.Bounds.Center.X, 0) - new Point(creditDim.X / 2, 0) + new Point(0, (int)posY);

                var creditRect = new Rectangle(creditPos, creditDim);
                if (_spriteBatch.GraphicsDevice.Viewport.Bounds.Intersects(creditRect))
                {
                    _spriteBatch.DrawString(_font, credit, creditPos.ToVector2(), Color.White);
                }

                posY -= creditDim.Y;
            }

            _spriteBatch.End();
        }

        void IGameComponent.Initialize()
        {
        }
    }
}
