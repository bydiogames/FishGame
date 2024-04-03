using FishGame.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FishGame.Animation
{
    internal class Sprite : IDrawable
    {
        public Texture2D Texture { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        private Rectangle _sourceRectangle;
        private int _yOffsetPx;
        private int _xOffsetPx;

        // Represents a sprite that consists of a single rectangle from a sprite sheet
        public Sprite(Texture2D texture, Rectangle sourceRectangle, int widthTiles, int heightTiles, int yOffsetPx = 0, int xOffsetPx = 0)
        {
            Texture = texture;
            Width = widthTiles;
            Height = heightTiles;
            _sourceRectangle = sourceRectangle;
            _yOffsetPx = yOffsetPx;
            _xOffsetPx = xOffsetPx;
        }

        public void Reset()
        {
        }

        public void Update()
        {
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 location)
        {

            Rectangle destinationRectangle = new Rectangle((int)(location.X * EntityConstants.TileWidthPx) + _xOffsetPx, (int)(location.Y * EntityConstants.TileHeightPx) + _yOffsetPx, Width * EntityConstants.TileWidthPx, Height * EntityConstants.TileHeightPx);

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
            spriteBatch.Draw(Texture, destinationRectangle, _sourceRectangle, Color.White);
            spriteBatch.End();
        }

        public bool IsFinished()
        {
            return false;
        }
    }
}
