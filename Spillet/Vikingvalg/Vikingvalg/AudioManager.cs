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
        public Dictionary<String, Song> _songList { get; set; } //Liste over sanger som kan spilles
        public Dictionary<String, SoundEffect> _soundEffectList { get; set; } //Liste over lydeffekter
        public Dictionary<String, SoundEffectInstance> _soundLoopInstanceList { get; set; } //Liste over instanser med repeterende lydeffekter
        public Queue<SoundEffectInstance> _soundQueue { get; set; } //Liste over lyder som spilles
        public Song _currentSong { get; set; } //Sangen som spilles
        public SoundEffect _currentAmbience { get; set; } //Bakgrunnslyden som spilles
        public SoundEffectInstance WalkLoop { get; set; } //Instans av gålyden

        public float FXVolume { get; set; } //Volum til lydeffekter
        public float MusicVolume { get; set; } // Volum til musikk
        private bool sfxPaused; //Hvorvidt lydeffekter er satt på pause

        public AudioManager(Game game)
            : base(game)
        {
            _songList = new Dictionary<string, Song>();
            _soundEffectList = new Dictionary<string, SoundEffect>();
            _soundLoopInstanceList = new Dictionary<string, SoundEffectInstance>();
            _soundQueue = new Queue<SoundEffectInstance>();
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
            //Legger til alle lydeffekter i listen over lydeffekter (Dette er veldig lite dynamisk, og vi ville ha endra det hadde vi hatt tid)
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
            
            //Legger til sang i listen over sanger og spiller den av
            _songList.Add("song1", Game.Content.Load<Song>(@"Audio/Music/Scabeater_Searching"));
            _currentSong = _songList["song1"];
            MediaPlayer.Play(_currentSong);

            //Legger til bakgrunnslyd
            _currentAmbience = Game.Content.Load<SoundEffect>(@"Audio/Ambience/forestAmb");
            AddSound(_currentAmbience, FXVolume, true);
            
            base.LoadContent();
        }
        public override void Update(GameTime gameTime)
        {
            //Sjekker om lydeffekten på starten har sluttet å spille, og fjerner den om den har det
            for (int i = 0; i < _soundQueue.Count; i++)
            {
                if (_soundQueue.Peek().State == SoundState.Stopped)
                    _soundQueue.Dequeue();
                else
                    break;
            }
            base.Update(gameTime);
        }
        /// <summary>
        ///Lager en instans av lyden som ble sendt inn, og spiller den av, legger instansen inn i soundQueue
        /// </summary>
        /// <param name="sound">Lydeffekt</param>
        /// <param name="volume">volum</param>
        /// <param name="loop">Hvorvidt den skal loopes</param>
        public void AddSound(SoundEffect sound, float volume, bool loop)
        {
            if (!sfxPaused)
            {
                SoundEffectInstance handle = sound.CreateInstance();
                handle.Volume = volume;
                if (loop) handle.IsLooped = true;
                handle.Play();
                _soundQueue.Enqueue(handle);
            }
        }
        /// <summary>
        /// Legger til en lydeffekt ved hjelp av navnet på lydeffekten
        /// Lydeffekten må allerede ha blitt lastet inn
        /// </summary>
        /// <param name="effectName">Navn på lydeffekten</param>
        public void AddSound(String effectName)
        {
            AddSound(getSoundFromDictionary(effectName), FXVolume, false);
        }
        /// <summary>
        /// Spiller av en loopet instans som allerede har blitt lastet inn
        /// </summary>
        /// <param name="instanceName">Navn på lyden som skal spilles</param>
        public void PlayLoop(String instanceName)
        {
            if (_soundLoopInstanceList[instanceName].State == SoundState.Stopped)
            {
                _soundQueue.Enqueue(_soundLoopInstanceList[instanceName]);
                _soundLoopInstanceList[instanceName].Play();
            }
        }
        /// <summary>
        /// Stopper en loopet instans som allerede spiller
        /// </summary>
        /// <param name="instanceName">Navnet på lyden som skal stoppes</param>
        public void StopLoop(String instanceName)
        {
            if (_soundLoopInstanceList[instanceName].State == SoundState.Playing)
            {
                _soundLoopInstanceList[instanceName].Stop();
            }
        }
        /// <summary>
        /// Henter ut lydeffekt fra listen over lydeffekter hvis den finnes,
        /// Hvis ikke, returner null
        /// </summary>
        /// <param name="effectName">Navn på lydeffekten som skal hentes ut</param>
        /// <returns></returns>
        public SoundEffect getSoundFromDictionary(String effectName)
        {
            if (_soundEffectList.ContainsKey(effectName))
            {
                return _soundEffectList[effectName];
            }
            return null;
            
        }
        /// <summary>
        /// Spiller av musikk
        /// </summary>
        public void PlayMusic()
        {
            MediaPlayer.Play(_currentSong);
        }
        /// <summary>
        /// Pauser musikk og lydeffekter
        /// </summary>
        public void PauseAll()
        {
            PauseMusic();
            PauseSounds();
        }
        /// <summary>
        /// pauser musikk
        /// </summary>
        public void PauseMusic()
        {
            MediaPlayer.Pause();
        }
        /// <summary>
        /// pauser lydeffekter
        /// </summary>
        public void PauseSounds()
        {
            sfxPaused = true;
            foreach (SoundEffectInstance item in _soundQueue)
            {
                if (item.State == SoundState.Playing)
                    item.Pause();
            }
        }
        /// <summary>
        /// Starter musikk og lydeffekter
        /// </summary>
        public void ResumeAll()
        {
            ResumeMusic();
            ResumeSounds();
        }
        /// <summary>
        /// Starter musikk
        /// </summary>
        public void ResumeMusic()
        {
            MediaPlayer.Resume();
        }
        /// <summary>
        /// Starter lydeffekter
        /// </summary>
        public void ResumeSounds()
        {
            foreach (SoundEffectInstance item in _soundQueue)
            {
                if (item.State == SoundState.Paused)
                    item.Resume();
            }
            sfxPaused = false;
        }
    }
}
