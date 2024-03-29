using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Reflection.Metadata.Ecma335;

namespace FishGame.Animation
{
    internal class SpriteAnimation
    {
        public Texture2D Texture { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool ShouldLoop { get; }

        private int _currentFrame;
        private int _totalFrames;
        private int _startingFrame;

        public SpriteAnimation(Texture2D texture, int rows, int columns, int totalFrames, int width, int height, int startingFrame = 0, bool shouldLoop = false)
        {
            Texture = texture;
            Rows = rows;
            Columns = columns;
            ShouldLoop = shouldLoop;
            Width = width;
            Height = height;
            _currentFrame = startingFrame;
            _totalFrames = totalFrames + startingFrame;
            _startingFrame = startingFrame;
        }

        public void Reset()
        {
            _currentFrame = _startingFrame;
        }

        public void Update()
        {
            if (IsFinished()) { return; }

            _currentFrame++;
            if (_currentFrame == _totalFrames)
            {
                _currentFrame = _startingFrame;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 location)
        {
            int width = Texture.Width / Columns;
            int height = Texture.Height / Rows;
            int row = _currentFrame / Columns;
            int column = _currentFrame % Columns;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, Width, Height);

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
            spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White);
            spriteBatch.End();
        }

        public bool IsFinished()
        {
            return !ShouldLoop && _currentFrame == _totalFrames - 1;
        }
    }
}
