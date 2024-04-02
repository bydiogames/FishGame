

using FishGame.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FishGame.Backgrounds
{
    internal class Background
    {
        private Texture2D _waterTexture;
        private Texture2D _landTexture;
        private Texture2D _dockTexture;
        private Texture2D _decorations;
        private Season _season;
        private Location _location;

        public Background(Season season) 
        {
            _season = season;
        }

        public void LoadPond(ContentManager content)
        {
            _location = Location.Pond;
            _waterTexture = content.Load<Texture2D>("General/water");
            _landTexture = content.Load<Texture2D>("Pond/land");
            _dockTexture = content.Load<Texture2D>("General/dock");

            // load decorations
            if (_season == Season.Spring)
            {
                _decorations = content.Load<Texture2D>("Pond/deco_spring");
            }
            else if (_season == Season.Summer)
            {
                _decorations = content.Load<Texture2D>("Pond/deco_summer");
            }
            else if (_season == Season.Fall)
            {
                _decorations = content.Load<Texture2D>("Pond/deco_fall");
            }
        }

        public void LoadRiver(ContentManager content)
        {
            _location = Location.River;
            _waterTexture = content.Load<Texture2D>("General/water");
            _landTexture = content.Load<Texture2D>("River/land");
            _dockTexture = content.Load<Texture2D>("General/dock");

            // load decorations
            if (_season == Season.Spring)
            {
                _decorations = content.Load<Texture2D>("River/deco_spring");
            }
            else if (_season == Season.Summer)
            {
                _decorations = content.Load<Texture2D>("River/deco_summer");
            }
            else if (_season == Season.Fall)
            {
                _decorations = content.Load<Texture2D>("River/deco_fall");
            }
        }

        public void LoadOcean(ContentManager content)
        {
            _location = Location.Ocean;
            _waterTexture = content.Load<Texture2D>("General/water");
            _landTexture = content.Load<Texture2D>("Ocean/land");
            _dockTexture = null;

            // load decorations
            if (_season == Season.Spring)
            {
                _decorations = content.Load<Texture2D>("Ocean/deco_spring");
            }
            else if (_season == Season.Summer)
            {
                _decorations = content.Load<Texture2D>("Ocean/deco_summer");
            }
            else if (_season == Season.Fall)
            {
                _decorations = content.Load<Texture2D>("Ocean/deco_fall");
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle destinationRectangle = new Rectangle(0, 2 * EntityConstants.TileHeightPx, EntityConstants.BackgroundWidthTiles * EntityConstants.TileWidthPx, 
                EntityConstants.BackgroundHeightTiles * EntityConstants.TileHeightPx);

            // Draw water
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
            spriteBatch.Draw(_waterTexture, destinationRectangle, null, Color.White);
            spriteBatch.End();

            // Draw land
            Color seasonColorMask = Utils.GetSeasonalColorMask(_season, _location);

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
            spriteBatch.Draw(_landTexture, destinationRectangle, null, seasonColorMask);
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
                Color decorationMask = _season == Season.Fall ? seasonColorMask : Color.White;
                spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
                spriteBatch.Draw(_decorations, destinationRectangle, null, decorationMask);
                spriteBatch.End();
            }
        }

    }
}
