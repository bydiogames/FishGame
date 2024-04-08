

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
        private Texture2D _trees;
        private Season _season;

        public Background(Season season) 
        {
            _season = season;
        }

        public void LoadPond(ContentManager content)
        {
            LoadRiverOrPond(content, "Pond");
        }

        public void LoadRiver(ContentManager content)
        {
            LoadRiverOrPond(content, "River");
        }

        public void LoadOcean(ContentManager content)
        {

            LoadOceanAssets(content);
        }

        private void LoadRiverOrPond(ContentManager content, string location)
        {
            _waterTexture = content.Load<Texture2D>("General/water");
            _dockTexture = content.Load<Texture2D>("General/dock");

            if (_season == Season.Spring)
            {
                _decorations = content.Load<Texture2D>($"{location}/{location}__Plants_Spring");
                _landTexture = content.Load<Texture2D>($"{location}/{location}__Land");
                _trees = content.Load<Texture2D>($"{location}/{location}__Trees_Spring");
            }
            else if (_season == Season.Summer)
            {
                _decorations = content.Load<Texture2D>($"{location}/{location}__Plants_Summer");
                _landTexture = content.Load<Texture2D>($"{location}/{location}__Land");
                _trees = content.Load<Texture2D>($"{location}/{location}__Trees_Summer");
            }
            else if (_season == Season.Fall)
            {
                _landTexture = content.Load<Texture2D>($"{location}/{location}__Land_Fall");
                _trees = content.Load<Texture2D>($"{location}/{location}__Trees_Fall");
            }
            else if (_season == Season.Winter)
            {
                _landTexture = content.Load<Texture2D>($"{location}/{location}__Land_Snow");
                _trees = content.Load<Texture2D>($"{location}/{location}__Trees_Winter");
            }
        }

        private void LoadOceanAssets(ContentManager content)
        {
            _waterTexture = content.Load<Texture2D>("General/water");
            _dockTexture = null;
            _trees = null;

            if (_season == Season.Spring)
            {
                _decorations = content.Load<Texture2D>("Ocean/Ocean__Plants_Spring");
                _landTexture = content.Load<Texture2D>($"Ocean/Ocean__Land");
            }
            else if (_season == Season.Summer)
            {
                _decorations = content.Load<Texture2D>($"Ocean/Ocean__Plants_Summer");
                _landTexture = content.Load<Texture2D>($"Ocean/Ocean__Land");
            }
            else if (_season == Season.Fall)
            {
                    _landTexture = content.Load<Texture2D>($"Ocean/Ocean__Land");
            }
            else if (_season == Season.Winter)
            {
                _landTexture = content.Load<Texture2D>($"Ocean/Ocean__Land_Snow");
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle destinationRectangle = new Rectangle(2 * EntityConstants.TileWidthPx, 2 * EntityConstants.TileHeightPx, EntityConstants.BackgroundWidthTiles * EntityConstants.TileWidthPx, 
                EntityConstants.BackgroundHeightTiles * EntityConstants.TileHeightPx);

            // Draw water
            spriteBatch.Draw(_waterTexture, destinationRectangle, null, Color.White);
            spriteBatch.Draw(_landTexture, destinationRectangle, null, Color.White);

            // Draw dock
            if(_dockTexture != null)
            {
                spriteBatch.Draw(_dockTexture, destinationRectangle, null, Color.White);
            }

            // Draw decorations
            if (_decorations != null)
            {
                spriteBatch.Draw(_decorations, destinationRectangle, null, Color.White);
            }

            // Draw trees
            if (_trees != null)
            {
                spriteBatch.Draw(_trees, destinationRectangle, null, Color.White);
            }
        }

    }
}
