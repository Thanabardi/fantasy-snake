using UnityEngine;

namespace Thanabardi.FantasySnake.Core.UI
{
    public class SettingPanel : MonoBehaviour, IUIPanel
    {
        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
    }
}