using UnityEngine;

namespace Thanabardi.FantasySnake.Core.UI
{
    public class SceneLoadPanel : MonoBehaviour, IUIPanel
    {
        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
    }
}