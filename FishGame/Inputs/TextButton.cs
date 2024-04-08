using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace FishGame.Inputs
{
    internal class TextButton
    {
        private MouseState _previousMouse;
        private MouseState _currentMouse;

        private bool _isHovering;

        private SpriteFont _font;
        private string _text;
        private float _scale;
        private Vector2 _position;
        private Vector2 _size;
        private Rectangle _rectangle;

        public event EventHandler Clicked;

        public Vector2 Position { get => _position; }

        public TextButton(SpriteFont font, Vector2 position, string text, float scale = 1f)
        {
            _font = font;
            _position = position;
            _text = text;
            _scale = scale;
            _size = _font.MeasureString(text) * new Vector2(scale, scale);
            _rectangle = new Rectangle((int)_position.X, (int)_position.Y, (int)(_size.X * scale), (int)(_size.Y * scale));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Update();
            var colour = Color.White;

            if (_isHovering)
                colour = Color.Gray;

            spriteBatch.DrawString(_font, _text, _position, colour, 0, Vector2.Zero, _scale, SpriteEffects.None, 0);
        }

        private void Update()
        {
            _currentMouse = Mouse.GetState();

            var mouseRectangle = new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1);

            _isHovering = false;

            if (mouseRectangle.Intersects(_rectangle))
            {
                _isHovering = true;
                if (_currentMouse.LeftButton == ButtonState.Released && _previousMouse.LeftButton == ButtonState.Pressed)
                {
                    Clicked?.Invoke(this, EventArgs.Empty);
                }
            }

            _previousMouse = _currentMouse;
        }
    }
}
