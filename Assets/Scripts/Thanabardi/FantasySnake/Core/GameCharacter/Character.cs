using System;
using Thanabardi.FantasySnake.Core.FantasySnakeSO;
using Thanabardi.FantasySnake.Core.GameWorld;
using Thanabardi.FantasySnake.Utility;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Thanabardi.FantasySnake.Core.GameCharacter
{
    public abstract class Character : WorldItem
    {
        [SerializeField]
        private StatusUIUtility _statusUIUtility;
        public StatusUIUtility StatusUIUtility => _statusUIUtility;
        [SerializeField]
        private CharacterClassSO _characterClass;
        public CharacterClassSO CharacterClass => _characterClass;
        public int Health { get; protected set; }
        public int Attack { get; protected set; }
        public int Defense { get; protected set; }

        public void Awake()
        {
            Health = 1;
            if (_characterClass != null)
            {
                Health = Random.Range(_characterClass.HealthStat.min, _characterClass.HealthStat.max);
                Attack = Random.Range(_characterClass.AttackStat.min, _characterClass.AttackStat.max);
                Defense = Random.Range(_characterClass.DefenseStat.min, _characterClass.DefenseStat.max);
            }
            _statusUIUtility = Instantiate(_statusUIUtility, Vector3.up, Quaternion.identity);
            _statusUIUtility.transform.SetParent(transform, false);
            _statusUIUtility.Initialize(_characterClass.ClassIcon, Attack, Defense, Health);
        }
    }
}