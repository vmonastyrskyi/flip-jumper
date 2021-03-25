using System;
using UnityEngine;

namespace Audio
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private Sound[] sounds;

        private void Awake()
        {
            foreach (var sound in sounds)
            {
                sound.audioSource = gameObject.AddComponent<AudioSource>();
                sound.audioSource.clip = sound.clip;
                sound.audioSource.outputAudioMixerGroup = sound.audioMixerGroup;
                sound.audioSource.playOnAwake = sound.playOnAwake;
                sound.audioSource.loop = sound.loop;
            }
        }

        public void Play(string name)
        {
            var sound = Array.Find(sounds, s => s.name == name);
            sound?.audioSource.Play();
        }

        public void Play(string name, float pitch)
        {
            var sound = Array.Find(sounds, s => s.name == name);
            if (sound != null)
            {
                sound.audioSource.pitch = pitch;
                sound.audioSource.Play();
            }
        }

        public void PauseAll()
        {
            foreach (var sound in sounds)
                sound.audioSource.Pause();
        }
        
        public void UnPauseAll()
        {
            foreach (var sound in sounds)
                sound.audioSource.UnPause();
        }
    }
}