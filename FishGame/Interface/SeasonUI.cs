using FishGame.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace FishGame.Interface
{
    internal class SeasonUI : Screen, IGameComponent
    {
        private SpriteBatch _spriteBatch;

        public SeasonUI(Game1 game, GraphicsDevice graphicsDevice, ContentManager contentManager) : base(game, graphicsDevice, contentManager)
        {
            _toolTip = new ToolTip();
        }

        private Texture2D _uiTex;
        private ToolTip _toolTip;

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

        public event Action<Location> LocationChanged;

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_uiTex, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 2f, SpriteEffects.None, 0);
        }

        void IGameComponent.Initialize()
        {
        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}
