using System;
namespace Thanabardi.FantasySnake.Core.GameWorld.Potion
{
    public class Potion : WorldItem
    {
        public override void OnHit(WorldItem other, Action onDestroy)
        {
            onDestroy?.Invoke();
            Destroy(gameObject);
        }
    }
}