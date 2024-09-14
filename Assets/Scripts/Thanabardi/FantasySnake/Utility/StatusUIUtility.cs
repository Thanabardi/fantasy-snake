using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Thanabardi.FantasySnake.Utility
{
    public class StatusUIUtility : MonoBehaviour
    {
        [SerializeField]
        private Image _classIcon;
        [SerializeField]
        private Sprite _barSection;
        [SerializeField]
        private Sprite _attackIcon;
        [SerializeField]
        private Sprite _defendIcon;
        [SerializeField]
        private GameObject _healthBar;
        [SerializeField]
        private GameObject _attackBar;
        [SerializeField]
        private GameObject _defendBar;

        private List<Image> _healthBarSections = new();

        public void Initialize(Sprite classIcon, int attackStat, int defendStat, int maxHealth)
        {
            _classIcon.sprite = classIcon;
            CreateHealthBar(maxHealth);
            CreateAttackBar(attackStat);
            CreateDefendBar(defendStat);
        }

        public void OnHealthUpdate(int health)
        {
            for (int i = 0; i < _healthBarSections.Count; i++)
            {
                if (i <= health - 1) // health start at 1
                {
                    _healthBarSections[i].color = Color.green;
                }
                else
                {
                    _healthBarSections[i].color = Color.black;
                }
            }
        }

        private void CreateHealthBar(int maxHealth)
        {
            for (int i = 0; i < maxHealth; i++)
            {
                Image bar = InstantiateImage(_barSection, _healthBar.transform, Color.green);
                _healthBarSections.Add(bar);
            }
        }

        private void CreateAttackBar(int attackStat)
        {
            for (int i = 0; i < attackStat; i++)
            {
                Image image = InstantiateImage(_attackIcon, _attackBar.transform, Color.red);
                image.gameObject.AddComponent<AspectRatioFitter>().aspectMode = AspectRatioFitter.AspectMode.HeightControlsWidth;
            }
        }

        private void CreateDefendBar(int defendStat)
        {
            for (int i = 0; i < defendStat; i++)
            {
                Image image = InstantiateImage(_defendIcon, _defendBar.transform, Color.blue);
                image.gameObject.AddComponent<AspectRatioFitter>().aspectMode = AspectRatioFitter.AspectMode.HeightControlsWidth;
            }
        }

        private Image InstantiateImage(Sprite icon, Transform parent, Color color)
        {
            GameObject gameObject = new();
            gameObject.transform.SetParent(parent.transform, false);
            Image image = gameObject.AddComponent<Image>();
            image.sprite = icon;
            image.color = color;
            return image;
        }
    }
}