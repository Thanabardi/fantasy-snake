using Thanabardi.FantasySnake.Core.GameState;
using Thanabardi.FantasySnake.Core.System;
using Thanabardi.FantasySnake.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Thanabardi.FantasySnake.Core.UI
{
    public class SettingPanel : MonoBehaviour, IUIPanel
    {
        [SerializeField]
        private Button _exitButton;
        [SerializeField]
        private Button _menuButton;

        [Header("Audio Volume")]
        [SerializeField]
        private Slider _masterVolumeSlider;
        [SerializeField]
        private Slider _musicVolumeSlider;
        [SerializeField]
        private Slider _ambientVolumeSlider;
        [SerializeField]
        private Slider _sfxVolumeSlider;

        [Header("Audio Volume Label")]
        [SerializeField]
        private TextMeshProUGUI _masterVolumeText;
        [SerializeField]
        private TextMeshProUGUI _musicVolumeText;
        [SerializeField]
        private TextMeshProUGUI _ambientVolumeText;
        [SerializeField]
        private TextMeshProUGUI _sfxVolumeText;

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        private void OnEnable()
        {
            _exitButton.Select();
            if (GameStateManager.Instance.CurrentState.StateID == (int)GameStates.State.Menu)
            {
                _menuButton.gameObject.SetActive(false);
            }

            _exitButton.onClick.AddListener(OnExitButtonClickedHandler);
            _menuButton.onClick.AddListener(OnMenuButtonClickedHandler);
            _masterVolumeSlider.onValueChanged.AddListener(OnMainVolumeChangedHandler);
            _musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChangedHandler);
            _ambientVolumeSlider.onValueChanged.AddListener(OnAmbientVolumeChangedHandler);
            _sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChangedHandler);

            // load setting data
            OnMainVolumeChangedHandler(GetVolume(SoundManager.AudioVolumeKey.MASTER_AUDIO_VOLUME_KEY));
            OnMusicVolumeChangedHandler(GetVolume(SoundManager.AudioVolumeKey.MUSIC_AUDIO_VOLUME_KEY));
            OnAmbientVolumeChangedHandler(GetVolume(SoundManager.AudioVolumeKey.AMBIENT_AUDIO_VOLUME_KEY));
            OnSFXVolumeChangedHandler(GetVolume(SoundManager.AudioVolumeKey.SFX_AUDIO_VOLUME_KEY));
        }

        private void OnDisable()
        {
            _exitButton.onClick.RemoveListener(OnExitButtonClickedHandler);
            _menuButton.onClick.RemoveListener(OnMenuButtonClickedHandler);
            _masterVolumeSlider.onValueChanged.RemoveListener(OnMainVolumeChangedHandler);
            _musicVolumeSlider.onValueChanged.RemoveListener(OnMusicVolumeChangedHandler);
            _ambientVolumeSlider.onValueChanged.RemoveListener(OnAmbientVolumeChangedHandler);
            _sfxVolumeSlider.onValueChanged.RemoveListener(OnSFXVolumeChangedHandler);
        }

        private void OnExitButtonClickedHandler()
        {
            UIManager.Instance.SetPanelActive(UIManager.UIKey.SettingPanel, false);
        }

        private void OnMenuButtonClickedHandler()
        {
            GameStateManager.Instance.GoToState((int)GameStates.State.Menu);
        }

        private void OnMainVolumeChangedHandler(float value)
        {
            SetVolume(value, _masterVolumeSlider, _masterVolumeText, SoundManager.AudioVolumeKey.MASTER_AUDIO_VOLUME_KEY);
        }

        private void OnMusicVolumeChangedHandler(float value)
        {
            SetVolume(value, _musicVolumeSlider, _musicVolumeText, SoundManager.AudioVolumeKey.MUSIC_AUDIO_VOLUME_KEY);
        }

        private void OnAmbientVolumeChangedHandler(float value)
        {
            SetVolume(value, _ambientVolumeSlider, _ambientVolumeText, SoundManager.AudioVolumeKey.AMBIENT_AUDIO_VOLUME_KEY);
        }

        private void OnSFXVolumeChangedHandler(float value)
        {
            SetVolume(value, _sfxVolumeSlider, _sfxVolumeText, SoundManager.AudioVolumeKey.SFX_AUDIO_VOLUME_KEY);
        }

        public void SetVolume(float volume, Slider slider, TextMeshProUGUI volumeText, string audioVolumeKey)
        {
            SoundManager.Instance.MainAudioMixer.SetFloat(audioVolumeKey, volume);
            slider.value = volume;
            volumeText.SetText(ConvertVolumeToPercentage(volume));
        }

        private float GetVolume(string audioVolumeKey)
        {
            if (SoundManager.Instance.MainAudioMixer.GetFloat(audioVolumeKey, out float volume))
            {
                return volume;
            }
            return 0;
        }

        private string ConvertVolumeToPercentage(float volume)
        {
            return ((volume + 80) * 100 / 80).ToString("n0") + "%";
        }
    }
}