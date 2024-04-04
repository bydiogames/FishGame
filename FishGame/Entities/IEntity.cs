using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FishGame.Entities
{
    internal interface IEntity
    {
        public int UpdateOrder { get; }

        public Vector2 Position { get; set; }

        public void Update(GameTime gameTime);

        public void Draw(SpriteBatch spriteBatch);
    }
}
