using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Vikingvalg
{
    public interface IManageAudio
    {
        Dictionary<String, Song> _songList{ get; set; }
        Dictionary<String, SoundEffect> _soundEffectList{ get; set; }
        List<IPlaySound> checkForSoundList { get; set; }
        Queue<SoundEffectInstance> _soundQueue{ get; set; }
        Song _currentSong{ get; set; }
        SoundEffect _currentAmbience { get; set; }

        float FXVolume{ get; set; }
        float MusicVolume{ get; set; }

        void AddSound(SoundEffect sound, float volume, bool loop);
        void AddSound(String effectName);
        SoundEffect getSoundFromDictionary(String effectName);
        void PlayLoop(String instanceName);
        void StopLoop(String instanceName);
        void LoadAudio(Sprite toLoad);
        void PlayMusic();
        void PauseAll();
        void PauseMusic();
        void PauseSounds();
        void ResumeAll();
        void ResumeMusic();
        void ResumeSounds();
        
    }
}
