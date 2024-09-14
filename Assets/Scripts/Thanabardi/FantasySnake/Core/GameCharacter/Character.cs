using System;
using Thanabardi.FantasySnake.Core.FantasySnakeSO;
using Thanabardi.FantasySnake.Core.GameWorld;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Thanabardi.FantasySnake.Core.GameCharacter
{
    public abstract class Character : WorldItem
    {
        [SerializeField]
        private CharacterClassSO _characterClass;
        public CharacterClassSO CharacterClass => _characterClass;
        public int Health { get; private set; }
        public int Attack { get; private set; }
        public int Defense { get; private set; }

        public event Action<int> OnhealthUpdate;
        public event Action<int> OnTakeDamage;

        public void Awake()
        {
            if (_characterClass != null)
            {
                Health = Random.Range(_characterClass.HealthStat.min, _characterClass.HealthStat.max);
                Attack = Random.Range(_characterClass.AttackStat.min, _characterClass.AttackStat.max);
                Defense = Random.Range(_characterClass.DefenseStat.min, _characterClass.DefenseStat.max);
            }
            else
            {
                Health = 1;
                Attack = 1;
                Defense = 1;
            }
        }

        protected virtual void TakeDamage(int damage)
        {
            UpdateHealth(Health - damage);
            OnTakeDamage?.Invoke(damage);
        }


        protected virtual void UpdateHealth(int health)
        {
            Health = Mathf.Max(0, health);
            OnhealthUpdate?.Invoke(health);
        }

        protected virtual void UpdateAttack(int attack)
        {
            Attack = attack;
        }

        protected virtual void UpdateDefense(int defense)
        {
            Defense = defense;
        }
    }
}