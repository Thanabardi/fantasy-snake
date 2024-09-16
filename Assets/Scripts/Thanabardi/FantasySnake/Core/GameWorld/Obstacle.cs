using System;
using UnityEngine;
namespace Thanabardi.FantasySnake.Core.GameWorld
{
    public class Obstacle : WorldItem
    {
        [SerializeField]
        private int _width;
        public int Width => _width;

        [SerializeField]
        private int _height;
        public int Height => _height;

        public override void OnHit(WorldItem other, Action onDestroy) { }
    }
}