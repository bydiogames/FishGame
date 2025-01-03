﻿using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishGame.Interface
{
    public abstract class Screen
    {
        protected ContentManager _content;
        protected GraphicsDevice _graphicsDevice;
        protected Game1 _game;

        public Screen(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
        {
            _game = game;
            _graphicsDevice = graphicsDevice;
            _content = content;
        }

        public abstract void Update(GameTime gameTime);

        public abstract void LoadContent();

        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}
