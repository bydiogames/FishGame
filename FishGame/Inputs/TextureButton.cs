using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace FishGame.Inputs
{
    internal class TextureButton
    {
        private MouseState _currentMouse;
        private MouseState _previousMouse;
        private bool _isHovering;

        private Texture2D _texture;

        public event EventHandler Pressed;

        private Rectangle _rectangle;
        public Rectangle Rectangle { get => _rectangle; }
        public bool IsHovering { get => _isHovering; }

        public TextureButton(Texture2D texture, Rectangle rectangle)
        {
            _texture = texture;
            _rectangle = rectangle;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Update();
            var colour = Color.White;

            if (_isHovering)
                colour = Color.Gray;

            spriteBatch.Draw(_texture, Rectangle, colour);
        }

        private void Update()
        {
            _currentMouse = Mouse.GetState();

            var mouseRectangle = new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1);

            _isHovering = false;

            if (mouseRectangle.Intersects(Rectangle))
            {
                _isHovering = true;

                if (_currentMouse.LeftButton == ButtonState.Released && _previousMouse.LeftButton == ButtonState.Pressed)
                {
                    Pressed?.Invoke(this, EventArgs.Empty);
                }
            }

            _previousMouse = _currentMouse;
        }
    }
}
