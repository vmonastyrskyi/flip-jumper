using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Audio

{
    [Serializable]
    public class Sound
    {
        public string name;

        public AudioClip clip;

        public AudioMixerGroup audioMixerGroup;

        public bool playOnAwake;

        public bool loop;

        [HideInInspector] public AudioSource audioSource;
    }
}