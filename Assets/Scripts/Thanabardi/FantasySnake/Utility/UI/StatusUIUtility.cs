using System.Collections.Generic;
using Thanabardi.FantasySnake.Core.GameCharacter;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Thanabardi.FantasySnake.Utility.UI
{
    public class StatusUIUtility : MonoBehaviour
    {
        [SerializeField]
        private Sprite _barSection;
        [SerializeField]
        private Sprite _attackIcon;
        [SerializeField]
        private Sprite _defenseIcon;

        [SerializeField]
        private GameObject _healthBar;
        [SerializeField]
        private GameObject _attackBar;
        [SerializeField]
        private GameObject _defenseBar;

        [SerializeField]
        private Image _classIcon;
        [SerializeField]
        private GameObject _popupStatusTextPrefab;

        private List<Image> _healthBarSections = new();
        private Character _character;

        public void Awake()
        {
            _character = FindObjectOfType<Character>();
            _classIcon.sprite = _character.CharacterClass.ClassIcon;
        }

        private void Start()
        {
            CreateHealthBar(_character.Health);
            CreateAttackBar(_character.Attack);
            CreateDefenseBar(_character.Defense);
        }

        private void OnEnable()
        {
            _character.OnhealthUpdate += OnHealthUpdateHandler;
            _character.OnTakeDamage += OnTakeDamageHandler;
        }

        private void OnDisable()
        {
            _character.OnhealthUpdate -= OnHealthUpdateHandler;
            _character.OnTakeDamage -= OnTakeDamageHandler;
        }

        private void OnHealthUpdateHandler(int health)
        {
            for (int i = 0; i < _healthBarSections.Count; i++)
            {
                if (i <= health - 1) // health start at 1
                {
                    _healthBarSections[i].color = Color.green;
                }
                else
                {
                    _healthBarSections[i].color = Color.gray;
                }
            }
        }

        private void OnTakeDamageHandler(int damage)
        {
            GameObject popupStatus = Instantiate(_popupStatusTextPrefab);
            TextMeshProUGUI popupStatusText = popupStatus.GetComponent<TextMeshProUGUI>();
            popupStatusText.transform.SetParent(transform, false);
            if (damage > 0)
            {
                popupStatusText.SetText($"-{damage}");
                popupStatusText.color = Color.red;
            }
            else
            {
                // when receive no damage (defence >= Attacker attack)
                popupStatusText.SetText($"Blocked");
                popupStatusText.color = Color.white;
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

        private void CreateDefenseBar(int defenseStat)
        {
            for (int i = 0; i < defenseStat; i++)
            {
                Image image = InstantiateImage(_defenseIcon, _defenseBar.transform, Color.blue);
                image.gameObject.AddComponent<AspectRatioFitter>().aspectMode = AspectRatioFitter.AspectMode.HeightControlsWidth;
            }
        }

        private Image InstantiateImage(Sprite icon, Transform parent, Color color)
        {
            GameObject gameObject = new(icon.name);
            gameObject.transform.SetParent(parent.transform, false);
            Image image = gameObject.AddComponent<Image>();
            image.sprite = icon;
            image.color = color;
            return image;
        }
    }
}