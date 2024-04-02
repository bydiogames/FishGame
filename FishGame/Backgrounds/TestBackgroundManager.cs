using FishGame.Inputs;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FishGame.Backgrounds
{
    internal class TestBackgroundManager
    {
        private Background _background;
        private Season _season;
        private Location _location;
        private ContentManager _contentManager;
        private KeyPress _rightKeyPress;
        private KeyPress _leftKeyPress;
        private KeyPress _upKeyPress;
        private KeyPress _downKeyPress;

        public TestBackgroundManager(Season season, Location location)
        {
            _season = season;
            _location = location;
            _background = new Background(season);
            _rightKeyPress = new KeyPress(Keys.Right);
            _leftKeyPress = new KeyPress(Keys.Left);
            _upKeyPress = new KeyPress(Keys.Up);
            _downKeyPress = new KeyPress(Keys.Down);
        }

        public void Load(ContentManager content)
        {
            _contentManager = content;
            ReloadBackground();
        }

        private void ReloadBackground()
        {
            if (_location == Location.Pond)
            {
                _background.LoadPond(_contentManager);
            }
            else if (_location == Location.River)
            {
                _background.LoadRiver(_contentManager);
            }
            else if (_location == Location.Ocean)
            {
                _background.LoadOcean(_contentManager);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _background.Draw(spriteBatch);
        }

        public void Update()
        {
            if(_rightKeyPress.Triggered())
            {
                NextLocation();
            }
            if(_leftKeyPress.Triggered())
            {
                PreviousLocation();
            }
            if(_upKeyPress.Triggered())
            {
                NextSeason();
            }
            if (_downKeyPress.Triggered())
            {
                PreviousSeason();
            }
        }

        private void NextSeason()
        {
            if (_season == Season.Winter)
            {
                _season = Season.Spring;
            }
            else
            {
                _season++;
            }

            _background = new Background(_season);
            ReloadBackground();
        }

        private void PreviousSeason()
        {
            if (_season == Season.Spring)
            {
                _season = Season.Winter;
            }
            else
            {
                _season--;
            }

            _background = new Background(_season);
            ReloadBackground();
        }

        public void NextLocation()
        {
            if(_location == Location.Ocean)
            {
                _location = Location.Pond;
            }
            else
            {
                _location++;
            }
            ReloadBackground();
        }

        public void PreviousLocation()
        {
            if (_location == Location.Pond)
            {
                _location = Location.Ocean;
            }
            else
            {
                _location--;
            }
            ReloadBackground();
        }
    }
}

