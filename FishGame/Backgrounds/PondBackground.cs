

using FishGame.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FishGame.Backgrounds
{
    internal class PondBackground : IBackground
    {
        private Texture2D _waterTexture;
        private Texture2D _landTexture;
        private Texture2D _decorations;
        private Season _season;

        public PondBackground(Season season) 
        {
            _season = season;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle destinationRectangle = new Rectangle(0, 0, EntityConstants.BackgroundWidthTiles * EntityConstants.TileWidthPx, 
                EntityConstants.BackgroundHeightTiles * EntityConstants.TileHeightPx);

            // Draw water
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
            spriteBatch.Draw(_waterTexture, destinationRectangle, null, Color.White);
            spriteBatch.End();

            // Draw land
            Color seasonColorMask = Color.White;
            if(_season == Season.Fall)
            {
                seasonColorMask = Color.Orange;
            }
            else if(_season == Season.Winter)
            {
                 seasonColorMask = Color.LightGray;
            }

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
            spriteBatch.Draw(_landTexture, destinationRectangle, null, seasonColorMask);
            spriteBatch.End();

            // Draw decorations
        }

        public void Load(ContentManager content)
        {
            _waterTexture = content.Load<Texture2D>("General/water");
            _landTexture = content.Load<Texture2D>("Pond/land");

            // load land

            // load decorations
            if(_season == Season.Spring)
            {
                _decorations = content.Load<Texture2D>("Pond/spring_deco");
            }
            else if(_season == Season.Summer)
            {
                _decorations = content.Load<Texture2D>("Pond/summer_deco");
            }
            else if (_season == Season.Fall)
            {
                _decorations = content.Load<Texture2D>("Pond/fall_deco");
            }
        }

    }
}
