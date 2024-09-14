using System;
using System.Collections.Generic;
using Thanabardi.FantasySnake.Core.GameCharacter;
using UnityEngine;

namespace Thanabardi.FantasySnake.Core.FantasySnakeSO
{
    [CreateAssetMenu(fileName = "CharacterClassSO", menuName = "ScriptableObject/CharacterClassSO")]
    public class CharacterClassSO : ScriptableObject
    {
        [SerializeField]
        private Sprite _classIcon;
        public Sprite ClassIcon => _classIcon;

        [SerializeField, Min(0)]
        private float _spawnRate;
        public float SpawnRate => _spawnRate;

        [Header("Health Stat")]
        [SerializeField, Min(1)]
        private int _minHealthStat = 1;
        [SerializeField, Min(1)]
        private int _maxHealthStat = 1;
        public (int min, int max) HealthStat
        {
            get => FormatStatTuple(_minHealthStat, _maxHealthStat);
        }

        [Header("Attack Stat")]
        [SerializeField, Min(0)]
        private int _minAttackStat;
        [SerializeField, Min(0)]
        private int _maxAttackStat;
        public (int min, int max) AttackStat
        {
            get => FormatStatTuple(_minAttackStat, _maxAttackStat);
        }

        [Header("Defense Stat")]
        [SerializeField, Min(0)]
        private int _minDefenseStat;
        [SerializeField, Min(0)]
        private int _maxDefenseStat;
        public (int min, int max) DefenseStat
        {
            get => FormatStatTuple(_minDefenseStat, _maxDefenseStat);
        }

        [Space(20)]
        [SerializeField]
        private DamageMultiplier[] _damageMultipliers;
        public Dictionary<CharacterClassSO, int> DamageMultipliers
        {
            get => FormatDamageMultiplierDict(_damageMultipliers);
        }

        [SerializeField]
        private Hero[] _heroPrefabs;
        public Hero[] HeroPrefabs => _heroPrefabs;

        [SerializeField]
        private Monster[] _monsterPrefabs;
        public Monster[] MonsterPrefabs => _monsterPrefabs;

        private (int min, int max) FormatStatTuple(int _minvalue, int _maxValue)
        {
            return (_minvalue, _maxValue);
        }

        private Dictionary<CharacterClassSO, int> FormatDamageMultiplierDict(DamageMultiplier[] damageMultipliers)
        {
            Dictionary<CharacterClassSO, int> multiplierDict = new();
            foreach (var item in damageMultipliers)
            {
                // ignore duplicate Character Class
                if (!multiplierDict.ContainsKey(item.CharacterClassSO))
                {
                    multiplierDict.Add(item.CharacterClassSO, item.DamageMultiply);
                }
            }
            return multiplierDict;
        }

        [Serializable]
        public class DamageMultiplier
        {
            public CharacterClassSO CharacterClassSO;
            [Min(1)]
            public int DamageMultiply = 1;
        }
    }
}