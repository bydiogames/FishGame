using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FishGame.Interface
{
    public class ToolTip
    {
        public ToolTip() 
        {
        }

        private SpriteFont _font;
        private Texture2D _square;
        public void Load(ContentManager content, GraphicsDevice graphics)
        {
            _font = content.Load<SpriteFont>("gamefont");

            _square = new Texture2D(graphics, 1, 1);
            _square.SetData<Color>(new Color[] { Color.White });
        }

        public void Draw(string tooltip, SpriteBatch spriteBatch)
        {
            var mouseState = Mouse.GetState();

            Vector2 stringDim = _font.MeasureString(tooltip);
            spriteBatch.Draw(
                _square,
                new Rectangle(
                    mouseState.Position - new Point(0, (int)stringDim.Y) - new Point(2, 2),
                    stringDim.ToPoint() + new Point(2, 2)
            ),
                Color.Brown
            );
            spriteBatch.DrawString(_font, tooltip, mouseState.Position.ToVector2() - new Vector2(0, stringDim.Y) * 1f, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
        }
    }
}
