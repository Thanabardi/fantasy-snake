using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Thanabardi.FantasySnake.Core.GameWorld.GameCharacter;
using Thanabardi.FantasySnake.Core.GameState;
using Thanabardi.FantasySnake.Core.GameWorld;
using Thanabardi.FantasySnake.Utility;
using UnityEngine;
using Thanabardi.FantasySnake.Core.GameWorld.GamePotion;

namespace Thanabardi.FantasySnake.Core.GameSystem
{
    public class GameManager : MonoBehaviour
    {
        private GridManager _gridManager;
        private SpawnWorldObjectUtility _spawnWorldObjectUtility;
        private PlayerActionManager _playerActionManager;

        private CinemachineVirtualCamera _virtualCamera;
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
                        case Potion potion:
                            OnHitPotion(player, potion);
                            _playerActionManager.PlayerMoveHandler(_playerQueue, gridTile);
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
            _playerActionManager.FlipLookAt(player, monster.transform.position);
            _playerActionManager.FlipLookAt(monster, player.transform.position);
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
                if (_gridManager.TryGetGridTile(monster, out GridTile gridTile))
                {
                    _spawnWorldObjectUtility.SpawnCharacters(typeof(Monster));
                    _gridManager.RemoveWorldItem(monster);
                    _spawnWorldObjectUtility.SpawnPotion(gridTile);
                }
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

        private void OnHitPotion(Hero player, Potion potion)
        {
            potion.OnHit(player, () =>
            {
                SoundManager.Instance.RandomPlaySoundOneshot(SoundManager.Instance.PotionDropSFX);
                _gridManager.RemoveWorldItem(potion);
                Destroy(potion.gameObject);
            });
        }

        private void InitializePlayer()
        {
            if (_gridManager.TryGetRandomEmptyTile(out GridTile playerTile))
            {
                Hero player = (Hero)_spawnWorldObjectUtility.SpawnCharacter(typeof(Hero), playerTile);
                _playerQueue.Enqueue(player);
                _playerActionManager.PlayerAddHandler(_playerQueue, playerTile);
                UpdateVirtualCamera();
            }
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