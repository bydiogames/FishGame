
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
        private SoundEffect _castSfx;
        private SoundEffect _reelSfx;

        private ContentManager _contentManager;
        public void Load(ContentManager contentManager)
        {
            _contentManager = contentManager;

            _fishPickupSfx = contentManager.Load<SoundEffect>("SFX/fish_pickup");
            _exclamationSfx = contentManager.Load<SoundEffect>("SFX/exclamation");
            _castSfx = contentManager.Load<SoundEffect>("SFX/cast");
            _reelSfx = contentManager.Load<SoundEffect>("SFX/reel");

            MediaPlayer.IsRepeating = true;
        }

        public void MuteSfx()
        {
            SoundEffect.MasterVolume = 0;
        }

        public void PlaySfx()
        {
            SoundEffect.MasterVolume = 1;
        }

        public void PlayMusic()
        {
            MediaPlayer.Volume = 1;
        }

        public void MuteMusic()
        {
            MediaPlayer.Volume = 0;
        }

        public void PlayFishPickupSfx()
        {
            _fishPickupSfx.Play();

        }

        public void PlayExclamationSfx()
        {
            _exclamationSfx.Play();
        }

        public void PlayCastSfx()
        {
            _castSfx.Play();
        }

        public void PlayReelSfx()
        {
            _reelSfx.Play();
        }

        public void UpdateSong(Season season)
        {
            Song song = _contentManager.Load<Song>($"Music/{season}");
            MediaPlayer.Play(song);
        }
    }
}
