using System;
using Thanabardi.FantasySnake.Core.GameWorld.GameCharacter;

namespace Thanabardi.FantasySnake.Core.GameWorld.GamePotion
{
    public class DefenseUpPotion : Potion
    {
        public override void OnHit(WorldItem other, Action onDestroy)
        {
            switch (other)
            {
                case Character character:
                    character.UpdateDefense(character.Defense + 1);
                    break;
                default:
                    break;
            }
            onDestroy?.Invoke();
        }
    }
}