using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace FishGame.Inputs
{
    internal class TextureToggleButton
    {
        private MouseState _currentMouse;

        private bool _isHovering;

        private MouseState _previousMouse;

        private Texture2D _onTexture;
        private Texture2D _offTexture;
        private bool _isOn;

        public event EventHandler ToggleOn;
        public event EventHandler ToggleOff;

        private Rectangle _rectangle;
        public Rectangle Rectangle { get => _rectangle; }

        public TextureToggleButton(Texture2D onTexture, Texture2D offTexture, Rectangle rectangle, bool defaultOn = true)
        {
            _onTexture = onTexture;
            _offTexture = offTexture;
            _rectangle = rectangle;
            _isOn = defaultOn;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Update();
            var colour = Color.White;

            if (_isHovering)
                colour = Color.Gray;

            spriteBatch.Draw(_isOn ? _onTexture : _offTexture, Rectangle, colour);
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
                    _isOn = !_isOn;
                    if(_isOn)
                    {
                        ToggleOn?.Invoke(this, EventArgs.Empty);
                    }
                    else
                    {
                        ToggleOff?.Invoke(this, EventArgs.Empty);
                    }
                }
            }

            _previousMouse = _currentMouse;
        }
    }
}
