using UnityEngine;
using Thanabardi.Generic.Utility;
using Thanabardi.FantasySnake.Core.UI;

namespace Thanabardi.FantasySnake.Core.System
{
    public class UIManager : MonoSingleton<UIManager>
    {
        [SerializeField]
        private MenuPanel _menuPanel;

        public override void Awake()
        {
            base.Awake();
        }

        public IUIPanel Show(UIKey uIKey)
        {
            switch (uIKey)
            {
                case UIKey.MenuPanel:
                    _menuPanel.SetActive(true);
                    _menuPanel.transform.SetAsLastSibling();
                    return _menuPanel;
                default:
                    Debug.LogError("UI Panel not found");
                    return null;
            }
        }

        public enum UIKey
        {
            MenuPanel
        }
    }
}