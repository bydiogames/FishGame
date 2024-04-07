using FishGame.Backgrounds;
using FishGame.Entities;
using FishGame.Inventory;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FishGame.Interface
{
    internal class LocationUI : IGameComponent, IDrawable
    {
        private SpriteBatch _spriteBatch;

        public LocationUI(SpriteBatch spriteBatch)
        {
            _spriteBatch = spriteBatch;
        }

        private Texture2D _uiTex;

        private SpriteFont _font;
        private SpriteFont _popupFont;

        private Texture2D _square;

        private struct LocationInfo
        {
            public Location location;
            public Rectangle hoverRect;
        };

        private LocationInfo[] _locations = new LocationInfo[3]
        {
            new LocationInfo
            {
                location = Location.Ocean,
                hoverRect = new Rectangle(120 * 2, 80 * 2, 40 * 2, 80 * 2),
            },
            new LocationInfo
            {
                location = Location.Pond,
                hoverRect = new Rectangle(180 * 2, 80 * 2, 40 * 2, 80 * 2),
            },
            new LocationInfo
            {
                location = Location.River,
                hoverRect = new Rectangle(240 * 2, 80 * 2, 40 * 2, 80 * 2),
            },
        };

        public void Load(ContentManager content, GraphicsDevice graphics)
        {
            {
                _uiTex = content.Load<Texture2D>("Main_ui2");
            }

            _font = content.Load<SpriteFont>("gamefont");
            _popupFont = content.Load<SpriteFont>("popup_font");

            _square = new Texture2D(graphics, 1, 1);
            _square.SetData<Color>(new Color[] { Color.White });
        }

        int IDrawable.DrawOrder => 50;

        private bool visible = true;

        public bool Visible
        {
            get => visible; set
            {
                visible = value;
                if(VisibleChanged != null)
                    VisibleChanged(this, EventArgs.Empty);
            }
        }

        public event EventHandler<EventArgs> DrawOrderChanged;

        public event EventHandler<EventArgs> VisibleChanged;

        public event Action<Location> LocationChanged;

        private int? lastHoverIdx;
        private TimeSpan hoverTime;

        private void DrawToolTip(string tooltip)
        {
            var mouseState = Mouse.GetState();

            Vector2 stringDim = _font.MeasureString(tooltip);
            _spriteBatch.Draw(
                _square,
                new Rectangle(
                    mouseState.Position - new Point(0, (int)stringDim.Y) - new Point(2, 2),
                    stringDim.ToPoint() + new Point(2, 2)
                ),
                Color.Brown
            );
            _spriteBatch.DrawString(_font, tooltip, mouseState.Position.ToVector2() - new Vector2(0, stringDim.Y) * 1f, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
        }

        TimeSpan _hoverTimer;

        void IDrawable.Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);

            _spriteBatch.Draw(_uiTex, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 2f, SpriteEffects.None, 0);

            var mouseState = Mouse.GetState();

            foreach(var location in _locations)
            {
                if (location.hoverRect.Contains(mouseState.Position))
                {
                    if (mouseState.LeftButton == ButtonState.Pressed)
                    {
                        _hoverTimer = TimeSpan.Zero;
                        LocationChanged(location.location);
                        goto finish_draw;
                    }

                    _hoverTimer += gameTime.ElapsedGameTime;

                    if (_hoverTimer.TotalSeconds > 1)
                    {
                        DrawToolTip(location.location.GetName());
                        goto finish_draw;
                    }

                    goto finish_draw;
                }
            }

            _hoverTimer = TimeSpan.Zero;

        finish_draw:
            _spriteBatch.End();
        }

        void IGameComponent.Initialize()
        {
        }
    }
}
