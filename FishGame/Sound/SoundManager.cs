
using FishGame.Backgrounds;
using FishGame.Entities;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;

namespace FishGame.Sound
{
    internal class SoundManager
    {

        private SoundEffect _fishPickupSfx;
        private SoundEffect _exclamationSfx;

        private ContentManager _contentManager;
        public void Load(ContentManager contentManager)
        {
            _contentManager = contentManager;

            _fishPickupSfx = contentManager.Load<SoundEffect>("SFX/fish_pickup");
            _exclamationSfx = contentManager.Load<SoundEffect>("SFX/exclamation");
            MediaPlayer.IsRepeating = true;
        }

        public void PlayFishPickupSfx()
        {
            _fishPickupSfx.Play();
        }

        public void PlayExclamationSfx()
        {
            _exclamationSfx.Play();
        }

        public void UpdateSong(Season season)
        {
            Song song = _contentManager.Load<Song>($"Music/{season}");
            MediaPlayer.Play(song);
        }
    }
}
