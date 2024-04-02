using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FishGame.Backgrounds
{
    internal interface IBackground
    {
        public void Load(ContentManager content);

        public void Draw(SpriteBatch spriteBatch);
    }
}
