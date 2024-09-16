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
            SoundManager.Instance.RandomPlaySoundOneshot(SoundManager.Instance.UIClickSFX);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnPointerEnterButton?.Invoke();
            SoundManager.Instance.RandomPlaySoundOneshot(SoundManager.Instance.UIHoverSFX);
        }

        public void OnSelect(BaseEventData eventData)
        {
            OnSelectButton?.Invoke();
            SoundManager.Instance.RandomPlaySoundOneshot(SoundManager.Instance.UIHoverSFX);
        }
    }
}