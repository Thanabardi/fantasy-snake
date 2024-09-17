using System;
using Thanabardi.FantasySnake.Core.FantasySnakeSO;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Thanabardi.FantasySnake.Core.GameWorld.GameCharacter
{
    public abstract class Character : WorldItem
    {
        [SerializeField]
        private CharacterClassSO _characterClass;
        public CharacterClassSO CharacterClass => _characterClass;

        public int Health { get; private set; }
        public int Attack { get; private set; }
        public int Defense { get; private set; }

        public event Action<int> OnHealthUpdate;
        public event Action<int> OnAttackUpdate;
        public event Action<int> OnDefenseUpdate;

        public event Action<int> OnGetHit;
        public event Action OnAttack;
        public event Action OnDied;

        private int _maxHealth;

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
            _maxHealth = Health;
        }

        public virtual void UpdateHealth(int health)
        {
            Health = Mathf.Clamp(health, 0, _maxHealth);
            OnHealthUpdate?.Invoke(health);
        }

        public virtual void UpdateAttack(int attack)
        {
            Attack = Mathf.Max(0, attack);
            OnAttackUpdate?.Invoke(Attack);
        }

        public virtual void UpdateDefense(int defense)
        {
            Defense = Mathf.Max(0, defense);
            OnDefenseUpdate?.Invoke(Defense);
        }

        protected virtual void CharacterAttack()
        {
            OnAttack?.Invoke();
        }

        protected virtual void TakeDamage(int damage, Action onDestroy)
        {
            OnGetHit?.Invoke(damage);
            if (damage > 0)
            {
                UpdateHealth(Health - damage);
            }
            if (damage >= Health)
            {
                onDestroy?.Invoke();
                OnDied?.Invoke();
            }
        }
    }
}