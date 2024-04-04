
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace FishGame.Sound
{
    internal class SoundManager
    {

        private SoundEffect _fishPickupSfx;
        public void Load(ContentManager contentManager)
        {
            _fishPickupSfx = contentManager.Load<SoundEffect>("SFX/fish_pickup");
        }

        public void PlayFishPickup()
        {
            _fishPickupSfx.Play();
        }
    }
}
