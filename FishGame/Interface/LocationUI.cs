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
    internal class LocationUI : Screen, IGameComponent
    {
        public LocationUI(Game1 game, GraphicsDevice graphicsDevice, ContentManager contentManager) : base(game, graphicsDevice, contentManager)
        {
            _toolTip = new ToolTip();
        }

        private Texture2D _uiTex;
        private ToolTip _toolTip;

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

        public override void LoadContent()
        {
            _uiTex = _content.Load<Texture2D>("Main_ui2");

            _toolTip.Load(_content, _graphicsDevice);
        }

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

        private int? lastHoverIdx;
        private TimeSpan hoverTime;
        TimeSpan _hoverTimer;

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_uiTex, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 2f, SpriteEffects.None, 0);

            var mouseState = Mouse.GetState();

            foreach(var location in _locations)
            {
                if (location.hoverRect.Contains(mouseState.Position))
                {
                    if (mouseState.LeftButton == ButtonState.Pressed)
                    {
                        _hoverTimer = TimeSpan.Zero;
                        _game.ChangeLocation(location.location);
                        _game.LoadMainUI();
                    }

                    _hoverTimer += gameTime.ElapsedGameTime;

                    if (_hoverTimer.TotalSeconds > 1)
                    {
                        _toolTip.Draw(location.location.GetName(), spriteBatch);
                    }
                }
            }

            _hoverTimer = TimeSpan.Zero;
        }

        void IGameComponent.Initialize()
        {
        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}
