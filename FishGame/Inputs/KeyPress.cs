using Microsoft.Xna.Framework.Input;

namespace FishGame.Inputs
{
    internal class KeyPress
    {
        private bool _prevKeyDown;
        private Keys _key;

        internal KeyPress(Keys key)
        {
            _prevKeyDown = false;
            _key = key;
        }

        internal bool Triggered()
        {
            bool currentState = Keyboard.GetState().IsKeyDown(_key);
            bool isKeyPressed = false;
            if (!_prevKeyDown && currentState)
            {
                isKeyPressed = true;
            }

            _prevKeyDown = currentState;
            return isKeyPressed;
        }
    }
}
