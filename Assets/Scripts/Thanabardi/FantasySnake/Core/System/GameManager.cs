using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Thanabardi.FantasySnake.Core.GameWorld.Character;
using Thanabardi.FantasySnake.Core.GameState;
using Thanabardi.FantasySnake.Core.GameWorld;
using Thanabardi.FantasySnake.Utility;
using UnityEngine;

namespace Thanabardi.FantasySnake.Core.System
{
    public class GameManager : MonoBehaviour
    {
        private CinemachineVirtualCamera _virtualCamera;
        private GridManager _gridManager;
        private SpawnWorldObjectUtility _spawnWorldObjectUtility;
        private PlayerActionManager _playerActionManager;

        private Queue<Hero> _playerQueue = new();

        private void Awake()
        {
            _virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
            _gridManager = FindObjectOfType<GridManager>();
            _spawnWorldObjectUtility = FindObjectOfType<SpawnWorldObjectUtility>();
            _playerActionManager = FindObjectOfType<PlayerActionManager>();
        }

        private void Start()
        {
            InitializePlayer();
        }

        private void InitializePlayer()
        {
            GridTile playerTile = _gridManager.GetRandomEmptyTile();
            Hero player = (Hero)_spawnWorldObjectUtility.SpawnCharacter(typeof(Hero), playerTile);
            _playerQueue.Enqueue(player);
            _playerActionManager.PlayerAddHandler(_playerQueue, playerTile);
            UpdateVirtualCamera();
        }

        public void RotateOrderLeftHandler()
        {
            Hero player = _playerQueue.Dequeue();
            _playerQueue.Enqueue(player);
            _playerActionManager.PlayerRotateOrderHandler(_playerQueue);
            UpdateVirtualCamera();
        }

        public void RotateOrderRightHandler()
        {
            // dequeue until the last Hero becomes the first
            for (int i = 0; i < _playerQueue.Count - 1; i++)
            {
                Hero player = _playerQueue.Dequeue();
                _playerQueue.Enqueue(player);
            }
            _playerActionManager.PlayerRotateOrderHandler(_playerQueue);
            UpdateVirtualCamera();
        }

        public void MovePlayerHandler(Vector2 direction)
        {
            if (_playerQueue.TryPeek(out Hero player))
            {
                if (_gridManager.MoveWorldItemBy(player, (int)direction.x, (int)direction.y, out GridTile gridTile))
                {
                    switch (gridTile.ContainedItem)
                    {
                        case Hero hero:
                            OnHitHero(hero, gridTile);
                            break;
                        case Monster monster:
                            OnHitMonster(player, monster);
                            break;
                        case Obstacle obstacle:
                            OnHitObstacle(player, obstacle);
                            break;
                        default:
                            _playerActionManager.PlayerMoveHandler(_playerQueue, gridTile);
                            break;
                    }
                }
            }
        }

        private void OnHitHero(Hero hero, GridTile gridTile)
        {
            if (_playerQueue.Contains(hero))
            {
                // prevent players from turning back and make game over when hitting the line.
                if (hero != _playerQueue.ElementAt(1))
                {
                    GameStateManager.Instance.GoToState((int)GameStates.State.Menu);
                }
                return;
            }
            _playerQueue.Enqueue(hero);
            _playerActionManager.PlayerAddHandler(_playerQueue, gridTile);
            UpdateVirtualCamera();
            _spawnWorldObjectUtility.SpawnCharacters(typeof(Hero));
        }

        private void OnHitMonster(Hero player, Monster monster)
        {
            bool removePlayer = false;
            bool removeMonster = false;
            player.OnHit(monster, () => { removePlayer = true; });
            monster.OnHit(player, () => { removeMonster = true; });
            if (removePlayer)
            {
                _playerQueue.Dequeue();
                if (_playerQueue.Count == 0)
                {
                    GameStateManager.Instance.GoToState((int)GameStates.State.Menu);
                    return;
                }
                _playerActionManager.PlayerDiedHandler(_playerQueue);
                _gridManager.RemoveWorldItem(player);
                UpdateVirtualCamera();
            }
            if (removeMonster)
            {
                // spawn new monster before remove to prevent spawing at the same location
                _spawnWorldObjectUtility.SpawnCharacters(typeof(Monster));
                _gridManager.RemoveWorldItem(monster);
            }
        }

        private void OnHitObstacle(Hero player, Obstacle obstacle)
        {
            player.OnHit(obstacle, () =>
            {
                _playerQueue.Dequeue();
                if (_playerQueue.Count == 0)
                {
                    GameStateManager.Instance.GoToState((int)GameStates.State.Menu);
                    return;
                }
                _playerActionManager.PlayerDiedHandler(_playerQueue);
                _gridManager.RemoveWorldItem(player);
                UpdateVirtualCamera();
            });
        }

        private void UpdateVirtualCamera()
        {
            if (_playerQueue.TryPeek(out Hero player))
            {
                _virtualCamera.Follow = player.transform;
            }
        }
    }
}