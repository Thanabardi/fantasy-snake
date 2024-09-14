using System;
using Thanabardi.FantasySnake.Core.GameWorld;
using UnityEngine;

namespace Thanabardi.FantasySnake.Core.GameCharacter
{
    public class Hero : Character
    {
        public override void OnHit(WorldItem other, Action onDestroy)
        {
            switch (other)
            {
                case Monster monster:
                    int multiplyer = 1;
                    if (CharacterClass.DamageMultipliers.TryGetValue(CharacterClass, out int multiply))
                    {
                        multiplyer = multiply;
                    }
                    int damage = Mathf.Max(0, monster.Attack - Defense) * multiplyer;
                    TakeDamage(damage);
                    Debug.Log($"{name}, Health:{Health}, Attack:{Attack}, Defense:{Defense}, Multiplyer:{multiplyer}, Damage:{damage}");
                    if (Health <= 0)
                    {
                        // hero died
                        onDestroy?.Invoke();
                        return;
                    }
                    break;
                default:
                    break;
            }
        }
    }
}