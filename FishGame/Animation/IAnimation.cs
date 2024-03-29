using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace FishGame.Animation
{
    public delegate void OnAnimationCompletion();
    internal interface IAnimation
    {
        public void Load(ContentManager content);

        public void Reset();

        public void Update(GameTime gameTime);

        public void Draw(SpriteBatch spriteBatch);
    }
}
