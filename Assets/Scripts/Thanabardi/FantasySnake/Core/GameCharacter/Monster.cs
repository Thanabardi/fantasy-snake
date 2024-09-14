using System;
using Thanabardi.FantasySnake.Core.GameWorld;
using UnityEngine;

namespace Thanabardi.FantasySnake.Core.GameCharacter
{
    public class Monster : Character
    {
        public override void OnHit(WorldItem other, Action onDestroy)
        {
            switch (other)
            {
                case Hero hero:
                    int multiplyer = 1;
                    if (CharacterClass.DamageMultipliers.TryGetValue(CharacterClass, out int multiply))
                    {
                        multiplyer = multiply;
                    }
                    int damage = Mathf.Max(0, hero.Attack - Defense) * multiplyer;
                    if (damage >= Health)
                    {
                        // monster died
                        onDestroy?.Invoke();
                        return;
                    }
                    Debug.Log($"{name}, Health:{Health}, Attack:{Attack}, Defense:{Defense}, Multiplyer:{multiplyer}, Damage:{damage}");
                    Health -= damage;
                    StatusUIUtility.OnHealthUpdate(Health);
                    break;
                default:
                    break;
            }
        }
    }
}