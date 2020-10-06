
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts
{
    [Serializable]
    public class Sound
    {
        public string Name;
        public AudioClip[] AudioClips;
        public AudioSource Source;
        public float RandomPitchOffset;
        public float OriginalPitch;
        public float Volume;

        public void Play()
        {
            Source.pitch = (OriginalPitch + Random.Range(-RandomPitchOffset, RandomPitchOffset)) * Time.timeScale;
            Source.PlayOneShot(AudioClips[Random.Range(0, AudioClips.Length)]);
        }
    }
}
