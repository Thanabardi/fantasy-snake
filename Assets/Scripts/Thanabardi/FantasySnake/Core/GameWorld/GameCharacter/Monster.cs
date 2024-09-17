using System;
using UnityEngine;

namespace Thanabardi.FantasySnake.Core.GameWorld.GameCharacter
{
    public class Monster : Character
    {
        public override void OnHit(WorldItem other, Action onDestroy)
        {
            switch (other)
            {
                case Hero hero:
                    CharacterAttack();
                    int damage = Mathf.Max(0, hero.Attack - Defense);
                    if (CharacterClass.DamageMultipliers.TryGetValue(CharacterClass, out int multiply))
                    {
                        damage *= multiply;
                    }
                    TakeDamage(damage, onDestroy);
                    break;
                case Obstacle obstacle:
                    // monster died
                    TakeDamage(Health, onDestroy);
                    break;
                default:
                    break;
            }
        }
    }
}