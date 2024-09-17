using UnityEngine;
using Thanabardi.Generic.Utility;
using Thanabardi.FantasySnake.Core.UI;

namespace Thanabardi.FantasySnake.Core.GameSystem
{
    public class UIManager : MonoSingleton<UIManager>
    {
        [SerializeField]
        private MenuPanel _menuPanel;
        [SerializeField]
        private SettingPanel _settingPanel;

        public override void Awake()
        {
            base.Awake();
        }

        public IUIPanel SetPanelActive(UIKey uIKey, bool isActive)
        {
            switch (uIKey)
            {
                case UIKey.MenuPanel:
                    _menuPanel.SetActive(isActive);
                    _menuPanel.transform.SetAsLastSibling();
                    return _menuPanel;
                case UIKey.SettingPanel:
                    _settingPanel.SetActive(isActive);
                    _settingPanel.transform.SetAsLastSibling();
                    return _settingPanel;
                default:
                    Debug.LogError("UI Panel not found");
                    return null;
            }
        }

        public enum UIKey
        {
            MenuPanel,
            SettingPanel
        }
    }
}