


using Microsoft.Xna.Framework;

namespace FishGame.Animation
{
    public class PositionManager
    {
        private Vector2 _startingPosition;
        private Vector2 _currentPosition;
        private int _frameCount;
        private int _currentFrame;
        private float _dX;
        private float _dY;
        public PositionManager(Vector2 startPosition, int frameCount, float dX = 0, float dY = 0)
        {
            _startingPosition = startPosition;
            _currentPosition = startPosition;
            _frameCount = frameCount;
            _currentFrame = 1;
            _dX = dX;
            _dY = dY;
        }

        public void Update()
        {
            if( _currentFrame == _frameCount ) { return;  }
            _currentFrame++;
            _currentPosition.X += _dX;
            _currentPosition.Y += _dY;
        }

        public void Update(int dX, int dY)
        {
            if (_currentFrame == _frameCount) { return; }
            _currentFrame++;
            _currentPosition.X += dX;
            _currentPosition.Y += dY;
        }

        public void Reset()
        {
            _currentFrame = 1;
            _currentPosition = _startingPosition;
        }

        public Vector2 GetCurrentPosition() { return _currentPosition; }
    }
}
