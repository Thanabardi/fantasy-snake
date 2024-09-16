using System;
using Thanabardi.FantasySnake.Core.GameWorld;
using UnityEngine;

namespace Thanabardi.FantasySnake.Core.GameWorld.GameCharacter
{
    public class Monster : Character
    {
        public override void OnHit(WorldItem other, Action onDestroy)
        {
            TurnLookAt(other.transform.position);
            switch (other)
            {
                case Hero hero:
                    int multiplyer = 1;
                    if (CharacterClass.DamageMultipliers.TryGetValue(CharacterClass, out int multiply))
                    {
                        multiplyer = multiply;
                    }
                    int damage = Mathf.Max(0, hero.Attack - Defense) * multiplyer;
                    CharacterAttack();
                    TakeDamage(damage);
                    if (Health <= 0)
                    {
                        // monster died
                        CharacterDied();
                        onDestroy?.Invoke();
                        return;
                    }
                    break;
                case Obstacle obstacle:
                    // monster died
                    CharacterDied();
                    onDestroy?.Invoke();
                    break;
                default:
                    break;
            }
        }
    }
}