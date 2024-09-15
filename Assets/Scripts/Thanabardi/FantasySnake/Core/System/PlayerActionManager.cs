using System.Collections.Generic;
using Thanabardi.FantasySnake.Core.GameCharacter;
using Thanabardi.FantasySnake.Core.GameWorld;
using Thanabardi.FantasySnake.Core.System;
using UnityEngine;

namespace Thanabardi.FantasySnake.Utility
{
    public class PlayerActionManager : MonoBehaviour
    {
        private GridManager _gridManager;

        private Queue<GridTile> _playerTileQueue; // a reverse queue to store player position

        private void Awake()
        {
            _gridManager = FindObjectOfType<GridManager>();

            _playerTileQueue = new();
        }

        public void PlayerMoveHandler(Queue<Hero> players, GridTile destination)
        {
            // on player move
            _playerTileQueue.Enqueue(destination);
            _playerTileQueue.Dequeue();
            UpdatePlayerPosition(players);
        }

        public void PlayerDiedHandler(Queue<Hero> players)
        {
            // on hero died
            _playerTileQueue.Dequeue();
            UpdatePlayerPosition(players);
        }

        public void PlayerAddHandler(Queue<Hero> players, GridTile destination)
        {
            // on collect new hero
            _playerTileQueue.Enqueue(destination);
            UpdatePlayerPosition(players);
        }

        public void PlayerRotateOrderHandler(Queue<Hero> players)
        {
            UpdatePlayerPosition(players);
        }

        private void UpdatePlayerPosition(Queue<Hero> players)
        {
            if (players.Count != _playerTileQueue.Count)
            {
                Debug.LogError("the number of Player Grid Tiles and Player in queue are mismatch");
                return;
            }

            GridTile[] playerTileArray = _playerTileQueue.ToArray();
            Hero[] playerArray = players.ToArray();
            int lenght = playerArray.Length;
            for (int i = 0; i < lenght; i++)
            {
                GridTile destination = playerTileArray[(lenght - 1) - i];
                playerArray[i].TurnLookAt(destination.transform.position);
                _gridManager.PlaceWorldItem(playerArray[i], destination);
            }
        }
    }
}