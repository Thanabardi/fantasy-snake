using System;
using UnityEngine;

namespace Thanabardi.FantasySnake.Core.GameWorld.GameCharacter
{
    public class Hero : Character
    {
        public override void OnHit(WorldItem other, Action onDestroy)
        {
            switch (other)
            {
                case Monster monster:
                    CharacterAttack();
                    int damage = Mathf.Max(0, monster.Attack - Defense);
                    if (CharacterClass.DamageMultipliers.TryGetValue(CharacterClass, out int multiply))
                    {
                        damage *= multiply;
                    }
                    TakeDamage(damage, onDestroy);
                    break;
                case Obstacle obstacle:
                    // hero died
                    TakeDamage(Health, onDestroy);
                    break;
                default:
                    break;
            }
        }
    }
}