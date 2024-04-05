
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace FishGame.Sound
{
    internal class SoundManager
    {

        private SoundEffect _fishPickupSfx;
        private SoundEffect _exclamationSfx;
        public void Load(ContentManager contentManager)
        {
            _fishPickupSfx = contentManager.Load<SoundEffect>("SFX/fish_pickup");
            _exclamationSfx = contentManager.Load<SoundEffect>("SFX/exclamation");
        }

        public void PlayFishPickupSfx()
        {
            _fishPickupSfx.Play();
        }

        public void PlayExclamationSfx()
        {
            _exclamationSfx.Play();
        }
    }
}
