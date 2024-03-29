using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace FishGame.Entities
{
    internal interface IEntity
    {
        public Vector2 Position { get; set; }

        public void Load(ContentManager contentManager);

        public void Update(GameTime gameTime);
    }
}
