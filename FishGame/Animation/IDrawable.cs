using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishGame.Animation
{
    internal interface IDrawable
    {
        public void Reset();
        public void Update();
        public void Draw(SpriteBatch spriteBatch, Vector2 location);
        public bool IsFinished();
    }
}
