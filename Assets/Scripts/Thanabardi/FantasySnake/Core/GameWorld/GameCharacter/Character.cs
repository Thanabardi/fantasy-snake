using System;
using Thanabardi.FantasySnake.Core.FantasySnakeSO;
using Thanabardi.FantasySnake.Core.GameWorld;
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
        public event Action OnDie;

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

        public void TurnLookAt(Vector3 direction)
        {
            if (direction.x > transform.position.x)
            {
                // other is on the right side
                GetComponent<SpriteRenderer>().flipX = true;
            }
            else if (direction.x < transform.position.x)
            {
                // other is on the left side
                GetComponent<SpriteRenderer>().flipX = false;
            }
        }

        public void CharacterAttack()
        {
            OnAttack?.Invoke();
        }

        protected void CharacterDied()
        {
            OnDie?.Invoke();
        }

        protected virtual void TakeDamage(int damage)
        {
            OnGetHit?.Invoke(damage);
            if (damage > 0)
            {
                UpdateHealth(Health - damage);
            }
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
    }
}