using UnityEngine;
using UnityEngine.Audio;

namespace Thanabardi.FantasySnake.Core.FantasySnakeSO
{
    [CreateAssetMenu(fileName = "AudioClipSO", menuName = "ScriptableObject/AudioClipSO")]
    public class AudioClipSO : ScriptableObject
    {
        [SerializeField]
        private AudioClip _audioClip;
        public AudioClip AudioClip => _audioClip;

        [SerializeField]
        private AudioMixerGroup _audioMixerGroup;
        public AudioMixerGroup AudioMixerGroup => _audioMixerGroup;

        [SerializeField]
        private bool _isLoop = false;
        public bool IsLoop => _isLoop;

        [SerializeField, Range(0f, 1f)]
        private float _volume = 1f;
        public float Volume => _volume;

        [SerializeField, Range(-3f, 3f)]
        private float _pitch = 1f;
        public float Pitch => _pitch;
    }
}