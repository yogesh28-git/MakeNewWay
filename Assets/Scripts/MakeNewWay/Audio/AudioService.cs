using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MakeNewWay
{
    public class AudioService : MonoSingletonGeneric<AudioService>
    {
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;
        [SerializeField] private Sound[] sounds;

        private Dictionary<SoundType, AudioClip> soundsDict = new Dictionary<SoundType, AudioClip>();

        private void Awake( )
        {
            base.Awake( );
            foreach ( var sound in sounds )
            {
                soundsDict.Add( sound.Type, sound.Clip );
            }
        }
        private void Start( )
        {
            StartMusic();
            ChangeMusic( MusicType.FIRST_MUSIC );
        }

        public void PlaySound( SoundType type )
        {
            AudioClip clip = GetClip( type );
            sfxSource.PlayOneShot( clip );
        }

        public void ChangeMusic( MusicType type )
        {
            switch ( type )
            {
                case MusicType.FIRST_MUSIC:
                    musicSource.volume = 0.4f;
                    musicSource.pitch = 1f;
                    break;
                case MusicType.SECOND_MUSIC:
                    musicSource.volume = 1f;
                    musicSource.pitch = 1f;
                    break;
                case MusicType.OVER_MUSIC:
                    musicSource.volume = 1f;
                    musicSource.pitch = 0.6f;
                    break;
            }
        }

        private AudioClip GetClip( SoundType type )
        {
            AudioClip clip = null;
            bool isClipAvailable = soundsDict.TryGetValue( type, out clip );
            if ( isClipAvailable )
            {
                return clip;
            }
            else
            {
                return sounds[0].Clip;
            }
        }

        private void StartMusic( )
        {
            AudioClip clip = GetClip( SoundType.THEME );
            musicSource.clip = clip;
            musicSource.Play( );
        }      
    }
}
