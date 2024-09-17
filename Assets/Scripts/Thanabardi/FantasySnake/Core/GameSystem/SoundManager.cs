using System.Collections.Generic;
using Thanabardi.FantasySnake.Core.FantasySnakeSO;
using Thanabardi.FantasySnake.Utility.Audio;
using Thanabardi.Generic.Utility;
using UnityEngine;
using UnityEngine.Audio;

namespace Thanabardi.FantasySnake.Core.GameSystem
{
    public class SoundManager : MonoSingleton<SoundManager>
    {
        [SerializeField]
        private AudioMixer _mainAudioMixer;
        public AudioMixer MainAudioMixer => _mainAudioMixer;

        [Header("Music Audio Clips")]
        public AudioClipSO MenuMusic;
        public AudioClipSO SettingMusic;
        public AudioClipSO GameplayMusic;


        [Header("Ambient Audio Clips")]
        public AudioClipSO CaveAMB;


        [Header("SFX Audio Clips")]
        public AudioClipSO[] WalkSFX;
        public AudioClipSO[] HitSFX;
        public AudioClipSO[] DeadSFX;
        public AudioClipSO[] PotionDropSFX;
        public AudioClipSO[] UIHoverSFX;
        public AudioClipSO[] UIClickSFX;

        private Dictionary<AudioClipSO, AudioSource> _audioSourceDict;
        private AudioSource _audioSource;

        private const int _LOG_FADE_STEP = 100;

        public override void Awake()
        {
            base.Awake();
            _audioSourceDict = new();
            _audioSource = GetComponent<AudioSource>();
        }

        public void PlaySound(AudioClipSO audioClipSO, int step = _LOG_FADE_STEP)
        {
            // play sound by add new AudioSource component into SoundManager
            if (_audioSourceDict.ContainsKey(audioClipSO))
            {
                Destroy(_audioSourceDict[audioClipSO]);
                _audioSourceDict.Remove(audioClipSO);
                Debug.LogWarning($"{audioClipSO.name} is currently being played");
            }
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            _audioSourceDict.Add(audioClipSO, audioSource);
            PlaySound(audioSource, audioClipSO);
            audioSource.volume = 0;
            StartCoroutine(FadeSound.Fade(audioSource, audioClipSO.Volume, step));
        }

        public void StopSound(AudioClipSO audioClipSO, int step = _LOG_FADE_STEP)
        {
            if (_audioSourceDict.TryGetValue(audioClipSO, out AudioSource audioSource))
            {
                _audioSourceDict.Remove(audioClipSO);
                StartCoroutine(FadeSound.Fade(audioSource, 0, step, delegate { Destroy(audioSource); }));
            }
        }

        public void RandomPlaySoundOneshot(AudioClipSO[] audioClipSOs)
        {
            // play sound one shot on AudioSource component
            AudioClipSO audioClipSO = audioClipSOs[0];
            if (audioClipSOs.Length > 1)
            {
                audioClipSO = audioClipSOs[Random.Range(0, audioClipSOs.Length)];
            }
            _audioSource.outputAudioMixerGroup = audioClipSO.AudioMixerGroup;
            _audioSource.volume = audioClipSO.Volume;
            _audioSource.PlayOneShot(audioClipSO.AudioClip, audioClipSO.Volume);
        }

        private void PlaySound(AudioSource audioSource, AudioClipSO audioClipSOs)
        {
            // play sound by apply soundClip configuration
            audioSource.clip = audioClipSOs.AudioClip;
            audioSource.outputAudioMixerGroup = audioClipSOs.AudioMixerGroup;
            audioSource.loop = audioClipSOs.IsLoop;
            audioSource.volume = audioClipSOs.Volume;
            audioSource.pitch = audioClipSOs.Pitch;
            audioSource.Play();
        }

        public static class AudioVolumeKey
        {
            public const string MASTER_AUDIO_VOLUME_KEY = "MasterVolume";
            public const string MUSIC_AUDIO_VOLUME_KEY = "MusicVolume";
            public const string AMBIENT_AUDIO_VOLUME_KEY = "AmbientVolume";
            public const string SFX_AUDIO_VOLUME_KEY = "SFXVolume";
        }
    }
}