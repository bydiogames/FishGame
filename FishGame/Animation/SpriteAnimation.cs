using FishGame.Entities;
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
        private int _yOffsetPx;
        private int _xOffsetPx;

        public SpriteAnimation(Texture2D texture, int rows, int columns, int totalFrames, int widthTiles, int heightTiles, int startingFrame = 0, bool shouldLoop = false, int yOffsetPx = 0, int xOffsetPx = 0)
        {
            Texture = texture;
            Rows = rows;
            Columns = columns;
            ShouldLoop = shouldLoop;
            Width = widthTiles;
            Height = heightTiles;
            _currentFrame = startingFrame;
            _totalFrames = totalFrames + startingFrame;
            _startingFrame = startingFrame;
            _yOffsetPx = yOffsetPx;
            _xOffsetPx = xOffsetPx;
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

        // TODO: add padding for sprites where the char is moved (e.g. the pickup and fishing animations have different "anchor points")
        public void Draw(SpriteBatch spriteBatch, Vector2 location)
        {
            int width = Texture.Width / Columns;
            int height = Texture.Height / Rows;
            int row = _currentFrame / Columns;
            int column = _currentFrame % Columns;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle((int)(location.X * EntityConstants.TileWidthPx) + _xOffsetPx, (int)(location.Y  * EntityConstants.TileHeightPx) + _yOffsetPx, Width * EntityConstants.TileWidthPx, Height * EntityConstants.TileHeightPx);

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
