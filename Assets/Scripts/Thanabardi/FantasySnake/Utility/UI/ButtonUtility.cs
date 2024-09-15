using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Thanabardi.FantasySnake.Utility.UI
{
    public class ButtonUtility : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, ISelectHandler
    {
        public Button Button;

        public event Action OnPointerClickButton;
        public event Action OnPointerEnterButton;
        public event Action OnSelectButton;

        public void OnPointerClick(PointerEventData eventData)
        {
            OnPointerClickButton?.Invoke();
            // play sound
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnPointerEnterButton?.Invoke();
            // play sound
        }

        public void OnSelect(BaseEventData eventData)
        {
            OnSelectButton?.Invoke();
            // play sound
        }
    }
}