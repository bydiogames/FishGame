

using FishGame.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Reflection;

namespace FishGame.Backgrounds
{
    internal class Background
    {
        private Texture2D _waterTexture;
        private Texture2D _landTexture;
        private Texture2D _dockTexture;
        private Texture2D _decorations;
        private Season _season;

        public Background(Season season) 
        {
            _season = season;
        }

        private void LoadLandAndWater(ContentManager content, string location)
        {
            _waterTexture = content.Load<Texture2D>("General/water");

            if (_season == Season.Spring)
            {
                _decorations = content.Load<Texture2D>($"{location}/deco_spring");
                _landTexture = content.Load<Texture2D>($"{location}/land");
            }
            else if (_season == Season.Summer)
            {
                _decorations = content.Load<Texture2D>($"{location}/deco_summer");
                _landTexture = content.Load<Texture2D>($"{location}/land");
            }
            else if (_season == Season.Fall)
            {
                // TODO: Fix the fall decoration assets now that we have better ones
                //_decorations = content.Load<Texture2D>($"{location}/deco_fall");
                _landTexture = content.Load<Texture2D>($"{location}/land_fall");
            }
            else if (_season == Season.Winter)
            {
                _landTexture = content.Load<Texture2D>($"{location}/land_snow");
            }
        }

        public void LoadPond(ContentManager content)
        {
            LoadLandAndWater(content, "Pond");
            _dockTexture = content.Load<Texture2D>("General/dock");
        }

        public void LoadRiver(ContentManager content)
        {
            LoadLandAndWater(content, "River");
            _dockTexture = content.Load<Texture2D>("General/dock");
        }

        public void LoadOcean(ContentManager content)
        {
            _dockTexture = null;
            LoadLandAndWater(content, "Ocean");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle destinationRectangle = new Rectangle(2 * EntityConstants.TileWidthPx, 2 * EntityConstants.TileHeightPx, EntityConstants.BackgroundWidthTiles * EntityConstants.TileWidthPx, 
                EntityConstants.BackgroundHeightTiles * EntityConstants.TileHeightPx);

            // Draw water
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
            spriteBatch.Draw(_waterTexture, destinationRectangle, null, Color.White);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
            spriteBatch.Draw(_landTexture, destinationRectangle, null, Color.White);
            spriteBatch.End();

            // Draw dock
            if(_dockTexture != null)
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
                spriteBatch.Draw(_dockTexture, destinationRectangle, null, Color.White);
                spriteBatch.End();
            }

            // Draw decorations
            if (_decorations != null)
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
                spriteBatch.Draw(_decorations, destinationRectangle, null, Color.White);
                spriteBatch.End();
            }
        }

    }
}
