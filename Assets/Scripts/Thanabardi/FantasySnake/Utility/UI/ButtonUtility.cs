using TMPro;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Thanabardi.FantasySnake.Utility.UI
{
    public class ButtonUtility : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
    {
        [SerializeField]
        private TextMeshProUGUI _buttonText;
        public Button Button;

        public event Action OnPointerLeftClickButton;
        public event Action OnPointerRightClickButton;
        public event Action OnPointerEnterButton;
        public event Action OnSelectButton;

        private Color _fontColor;

        private void Awake()
        {
            if (_buttonText) { _fontColor = _buttonText.color; }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                OnPointerLeftClickButton?.Invoke();
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                OnPointerRightClickButton?.Invoke();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnPointerEnterButton?.Invoke();
            SetSelectColor(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            SetSelectColor(false);
        }

        public void OnSelect(BaseEventData eventData)
        {
            OnSelectButton?.Invoke();
            SetSelectColor(true);
        }

        public void OnDeselect(BaseEventData eventData)
        {
            SetSelectColor(false);
        }

        private void OnDisable()
        {
            SetSelectColor(false);
        }

        private void SetSelectColor(bool isSelect)
        {
            if (isSelect)
            {
                // playsound
                if (_buttonText) { _buttonText.color = Color.cyan; }
            }
            else
            {
                if (_buttonText) { _buttonText.color = _fontColor; }
            }
        }
    }
}