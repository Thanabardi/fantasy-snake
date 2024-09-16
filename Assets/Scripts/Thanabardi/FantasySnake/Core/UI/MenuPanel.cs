using System;
using UnityEngine;
using UnityEngine.UI;

namespace Thanabardi.FantasySnake.Core.UI
{
    public class MenuPanel : MonoBehaviour, IUIPanel
    {
        [SerializeField]
        private Button _playButton;

        [SerializeField]
        private Button _settingButton;

        [SerializeField]
        private Button _exitButton;

        public event Action OnPlayButtonClicked;
        public event Action OnSettingButtonClicked;
        public event Action OnExitButtonClicked;

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        private void OnEnable()
        {
            _playButton.Select();

            _playButton.onClick.AddListener(OnPlayButtonClickedHandler);
            _settingButton.onClick.AddListener(OnSettingButtonClickedHandler);
            _exitButton.onClick.AddListener(OnExitButtonClickedHandler);
        }

        private void OnDisable()
        {
            _playButton.onClick.RemoveListener(OnPlayButtonClickedHandler);
            _settingButton.onClick.RemoveListener(OnSettingButtonClickedHandler);
            _exitButton.onClick.RemoveListener(OnExitButtonClickedHandler);
        }

        private void OnPlayButtonClickedHandler()
        {
            OnPlayButtonClicked?.Invoke();
        }

        private void OnSettingButtonClickedHandler()
        {
            OnSettingButtonClicked?.Invoke();
            SetActive(false);
        }

        private void OnExitButtonClickedHandler()
        {
            OnExitButtonClicked?.Invoke();
        }
    }
}