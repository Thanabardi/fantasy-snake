using System;
using UnityEngine;

namespace Thanabardi.FantasySnake.Core.GameWorld
{
    public abstract class WorldItem : MonoBehaviour
    {
        public abstract void OnHit(WorldItem other, Action onDestroy);
    }
}