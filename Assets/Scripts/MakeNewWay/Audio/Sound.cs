using UnityEngine;

namespace MakeNewWay
{
    [System.Serializable]
    public class Sound
    {
        public SoundType Type { get { return type; } private set { } }
        public AudioClip Clip { get { return clip; } private set { } }

        [SerializeField] private SoundType type;
        [SerializeField] private AudioClip clip;
    }
}
