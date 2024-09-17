using System.Collections.Generic;
using Thanabardi.FantasySnake.Core.GameWorld.GameCharacter;
using Thanabardi.FantasySnake.Core.GameWorld;
using UnityEngine;

namespace Thanabardi.FantasySnake.Core.GameSystem
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
            _playerTileQueue.Dequeue();
            _playerTileQueue.Enqueue(destination);
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
            _gridManager.RemoveWorldItem(destination.ContainedItem); // to prevent when the hero stays at same position
            _playerTileQueue.Enqueue(destination);
            UpdatePlayerPosition(players);
        }

        public void PlayerRotateOrderHandler(Queue<Hero> players)
        {
            UpdatePlayerPosition(players);
        }

        public void FlipLookAt(Character character, Vector3 target)
        {
            if (target.x > character.transform.position.x)
            {
                // target is on the right side
                character.GetComponent<SpriteRenderer>().flipX = true;
            }
            else if (target.x < character.transform.position.x)
            {
                // target is on the left side
                character.GetComponent<SpriteRenderer>().flipX = false;
            }
        }

        private void UpdatePlayerPosition(Queue<Hero> players)
        {
            if (players.Count != _playerTileQueue.Count)
            {
                Debug.LogError("the number of Player Grid Tiles and Player in queue are mismatch");
                return;
            }
            SoundManager.Instance.RandomPlaySoundOneshot(SoundManager.Instance.WalkSFX);
            GridTile[] playerTileArray = _playerTileQueue.ToArray();
            Hero[] playerArray = players.ToArray();
            int lenght = playerArray.Length;
            for (int i = 0; i < lenght; i++)
            {
                // the direction must follow playerArray unless the first tile will update incorrectly
                GridTile destination = playerTileArray[(lenght - 1) - i];
                FlipLookAt(playerArray[i], destination.transform.position);
                playerArray[i].transform.position = destination.transform.position;
                _gridManager.AddItemOnTile(playerArray[i], destination);
            }
        }
    }
}