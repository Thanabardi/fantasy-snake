using System;
using Thanabardi.FantasySnake.Core.GameWorld.GameCharacter;

namespace Thanabardi.FantasySnake.Core.GameWorld.GamePotion
{
    public class HealingPotion : Potion
    {
        public override void OnHit(WorldItem other, Action onDestroy)
        {
            switch (other)
            {
                case Character character:
                    character.UpdateHealth(character.Health + 1);
                    break;
                default:
                    break;
            }
            onDestroy?.Invoke();
        }
    }
}