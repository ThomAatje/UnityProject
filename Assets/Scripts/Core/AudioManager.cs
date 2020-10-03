using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class AudioManager : MonoBehaviour
    {

        [SerializeField] private List<Sound> _sounds = new List<Sound>();
        private Dictionary<string, Sound> _soundDictionary;

        public void Start()
        {
            _soundDictionary = new Dictionary<string, Sound>();

            foreach (var sound in _sounds)
            {
                if (sound.Source == null)
                    sound.Source = gameObject.AddComponent<AudioSource>();
                
                if (sound.OriginalPitch == 0)
                    sound.OriginalPitch = sound.Source.pitch;

                sound.Source.volume = sound.Volume;
                _soundDictionary.Add(sound.Name.ToLower(), sound);
            }
        }

        public void Play(string soundName)
        {
            var sound = _soundDictionary[soundName.ToLower()];

            if (sound == null)
            {
                Debug.LogError($"Failed to play sound: {soundName}, sound does not exist.");
                return;
            }

            sound.Play();
        }
    }
}
