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
    public class AudioManager : DrawableGameComponent, IManageAudio
    {
        public Dictionary<String, Song> _songList { get; set; }
        public Dictionary<String, SoundEffect> _soundEffectList { get; set; }
        public Dictionary<String, SoundEffectInstance> _soundLoopInstanceList { get; set; }
        public Queue<SoundEffectInstance> _soundQueue { get; set; }
        public List<IPlaySound> checkForSoundList { get; set; }
        public Song _currentSong { get; set; }
        public SoundEffect _currentAmbience { get; set; }
        public SoundEffectInstance WalkLoop { get; set; }

        public float FXVolume { get; set; }
        public float MusicVolume { get; set; }
        private bool bPaused;

        public AudioManager(Game game)
            : base(game)
        {
            _songList = new Dictionary<string, Song>();
            _soundEffectList = new Dictionary<string, SoundEffect>();
            _soundLoopInstanceList = new Dictionary<string, SoundEffectInstance>();
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
            _soundLoopInstanceList.Add("walk",Game.Content.Load<SoundEffect>(@"Audio/SoundEffects/player/walkTest").CreateInstance());
            _soundLoopInstanceList["walk"].Volume = 0.1f;
            _soundEffectList.Add("player/attack1", Game.Content.Load<SoundEffect>(@"Audio/SoundEffects/player/swordSlash1"));
            _soundEffectList.Add("player/attack2", Game.Content.Load<SoundEffect>(@"Audio/SoundEffects/player/swordSlash2"));
            _soundEffectList.Add("player/blockHit", Game.Content.Load<SoundEffect>(@"Audio/SoundEffects/player/shieldHit"));
            _soundEffectList.Add("player/clang1", Game.Content.Load<SoundEffect>(@"Audio/SoundEffects/player/clang1"));
            _soundEffectList.Add("player/clang2", Game.Content.Load<SoundEffect>(@"Audio/SoundEffects/player/clang2"));
            _soundEffectList.Add("player/clang3", Game.Content.Load<SoundEffect>(@"Audio/SoundEffects/player/clang3"));
            _soundEffectList.Add("wolf/attack1", Game.Content.Load<SoundEffect>(@"Audio/SoundEffects/wolf/attack1"));
            _soundEffectList.Add("wolf/attack2", Game.Content.Load<SoundEffect>(@"Audio/SoundEffects/wolf/attack2"));
            _soundEffectList.Add("blob/attack1", Game.Content.Load<SoundEffect>(@"Audio/SoundEffects/blob/attack1"));
            _soundEffectList.Add("blob/attack2", Game.Content.Load<SoundEffect>(@"Audio/SoundEffects/blob/attack2"));
            _soundEffectList.Add("stone/crumble", Game.Content.Load<SoundEffect>(@"Audio/SoundEffects/stone/crumble1"));
            _soundEffectList.Add("stone/money", Game.Content.Load<SoundEffect>(@"Audio/SoundEffects/stone/money"));
            /*_soundEffectList.Add("wolf/growl", Game.Content.Load<SoundEffect>(@"Audio/SoundEffects/wolf/taunt"));
            _soundEffectList.Add("blob/growl", Game.Content.Load<SoundEffect>(@"Audio/SoundEffects/wolf/taunt"));*/
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

                /*if (soundPlayingObject is Player && soundPlayingObject.currentSoundEffect != "walk") StopWalk();
                if (soundPlayingObject is Player && soundPlayingObject.currentSoundEffect == "walk" && !bPaused) AddWalk();
                else if (soundPlayingObject.currentSoundEffect !="" && soundPlayingObject.currentSoundEffect != "walk")
                {
                    AddSound(soundPlayingObject.Directory + "/" + soundPlayingObject.currentSoundEffect);
                    soundPlayingObject.currentSoundEffect = "";
                }*/
            }
            for (int i = 0; i < _soundQueue.Count; i++)
            {
                if (_soundQueue.Peek().State == SoundState.Stopped)
                    _soundQueue.Dequeue();
                else
                    break;
            }
            base.Update(gameTime);
        }
        public void AddSound(SoundEffect sound, float volume, bool loop)
        {
            if (!bPaused)
            {
                SoundEffectInstance handle = sound.CreateInstance();
                handle.Volume = volume;
                if (loop) handle.IsLooped = true;
                handle.Play();
                _soundQueue.Enqueue(handle);
            }
        }
        public void AddSound(String effectName)
        {
            AddSound(getSoundFromDictionary(effectName), FXVolume, false);
        }
        public void PlayLoop(String instanceName)
        {
            if (_soundLoopInstanceList[instanceName].State == SoundState.Stopped)
            {
                _soundQueue.Enqueue(_soundLoopInstanceList[instanceName]);
                _soundLoopInstanceList[instanceName].Play();
            }
        }
        public void StopLoop(String instanceName)
        {
            if (_soundLoopInstanceList[instanceName].State == SoundState.Playing)
            {
                _soundLoopInstanceList[instanceName].Stop();
            }
        }
        public SoundEffect getSoundFromDictionary(String effectName)
        {
            if (_soundEffectList.ContainsKey(effectName))
            {
                return _soundEffectList[effectName];
            }
            return null;
            
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
            MediaPlayer.Pause();
        }

        public void PauseSounds()
        {
            bPaused = true;
            foreach (SoundEffectInstance item in _soundQueue)
            {
                if (item.State == SoundState.Playing)
                    item.Pause();
            }
        }

        public void ResumeAll()
        {
            throw new NotImplementedException();
        }

        public void ResumeMusic()
        {
            MediaPlayer.Resume();
        }

        public void ResumeSounds()
        {
            foreach (SoundEffectInstance item in _soundQueue)
            {
                if (item.State == SoundState.Paused)
                    item.Resume();
            }
            bPaused = false;
        }
    }
}
