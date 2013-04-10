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
    //Satt til DrawableGameComponent for å bruke override loadcontent
    class AudioManager : DrawableGameComponent, IManageAudio
    {
        public Dictionary<String, Song> _songList { get; set; }
        public Dictionary<String, SoundEffect> _soundEffectList { get; set; }
        public Queue<SoundEffectInstance> _soundQueue { get; set; }
        public List<IPlaySound> checkForSoundList { get; set; }
        public Song _currentSong { get; set; }
        public SoundEffect _currentAmbience { get; set; }
        public SoundEffectInstance AmbienceInstance { get; set; }

        public float FXVolume { get; set; }
        public float MusicVolume { get; set; }

        public AudioManager(Game game)
            : base(game)
        {
            _songList = new Dictionary<string, Song>();
            _soundEffectList = new Dictionary<string, SoundEffect>();
            _soundQueue = new Queue<SoundEffectInstance>();
            checkForSoundList = new List<IPlaySound>();
        }

        public override void Initialize()
        {
            MusicVolume = 0.2f;
            FXVolume = 0.3f;
            MediaPlayer.Volume = MusicVolume;
            MediaPlayer.IsRepeating = true;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _soundEffectList.Add("player/attack1", Game.Content.Load<SoundEffect>(@"Audio/SoundEffects/bite1"));
            _soundEffectList.Add("player/attack2", Game.Content.Load<SoundEffect>(@"Audio/SoundEffects/bite2"));
            _soundEffectList.Add("player/clang", Game.Content.Load<SoundEffect>(@"Audio/SoundEffects/clang"));
            _soundEffectList.Add("wolf/attack1", Game.Content.Load<SoundEffect>(@"Audio/SoundEffects/bite1"));
            _soundEffectList.Add("blob/attack1", Game.Content.Load<SoundEffect>(@"Audio/SoundEffects/bite2"));
            _currentSong = Game.Content.Load<Song>(@"Audio/Music/Scabeater_Searching");
            _currentAmbience = Game.Content.Load<SoundEffect>(@"Audio/Ambience/forestAmb");
            AddSound(_currentAmbience, FXVolume, true);
            MediaPlayer.Play(_currentSong);
            base.LoadContent();
        }
        public override void Update(GameTime gameTime)
        {
            foreach (IPlaySound soundPlayingObject in checkForSoundList)
            {
                if (!(soundPlayingObject.currentSoundEffect.Equals("")))
                {
                    AddSound(soundPlayingObject.Directory+"/"+soundPlayingObject.currentSoundEffect);
                    soundPlayingObject.currentSoundEffect = "";
                }
            }
            base.Update(gameTime);
        }
        public void AddSound(Microsoft.Xna.Framework.Audio.SoundEffect sound, float volume, bool loop)
        {
            SoundEffectInstance handle = sound.CreateInstance();
            handle.Volume = volume;
            if (loop) handle.IsLooped = true;
            handle.Play();
            _soundQueue.Enqueue(handle);
        }
        public void addSoundPlayingObject(IPlaySound soundPlayingObject)
        {
            checkForSoundList.Add(soundPlayingObject);
        }
        public void AddSound(String effectName)
        {
            AddSound(getSoundFromDictionary(effectName), FXVolume, false);
        }
        public SoundEffect getSoundFromDictionary(String effectName)
        {
            return _soundEffectList[effectName];
        }
        public void LoadAudio(Sprite toLoad)
        {
            throw new NotImplementedException();
        }

        public void PlayMusic()
        {
            MediaPlayer.Play(_currentSong);
        }

        public void PauseAll()
        {
            throw new NotImplementedException();
        }

        public void PauseMusic()
        {
            throw new NotImplementedException();
        }

        public void PauseSounds()
        {
            throw new NotImplementedException();
        }

        public void ResumeAll()
        {
            throw new NotImplementedException();
        }

        public void ResumeMusic()
        {
            throw new NotImplementedException();
        }

        public void ResumeSounds()
        {
            throw new NotImplementedException();
        }
    }
}
