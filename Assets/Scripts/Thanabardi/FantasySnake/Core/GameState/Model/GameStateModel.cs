using Thanabardi.FantasySnake.Core.GameScene;
using Thanabardi.FantasySnake.Core.NewInputSystem;
using Thanabardi.FantasySnake.Core.System;
using Thanabardi.FantasySnake.Utility;
using Thanabardi.Generic.Core.StateSystem;
using UnityEngine;

namespace Thanabardi.FantasySnake.Core.GameState.Model
{
    public class GameStateModel : StateModel
    {
        private GameManager _gameManager;
        private PlayerActionUtility _playerActionUtility;

        public GameStateModel() : base((int)GameStates.State.GamePlay, nameof(GameStateModel)) { }
        public override void OnStateIn()
        {
            base.OnStateIn();
            GameSceneManager.Instance.GoToScene(GameSceneManager.SceneKey.GameScene, () =>
            {
                InputManager.Instance.EnableGameAction(true);
                _gameManager = GameObject.FindObjectOfType<GameManager>();
                _playerActionUtility = GameObject.FindObjectOfType<PlayerActionUtility>();

                _playerActionUtility.OnPlayerMove += _gameManager.MovePlayerHandler;
                _playerActionUtility.OnRotateOrderLeft += _gameManager.RotateOrderLeftHandler;
                _playerActionUtility.OnRotateOrderRight += _gameManager.RotateOrderRightHandler;
            });
        }


        public override void OnStateOut()
        {
            base.OnStateOut();
            InputManager.Instance.EnableGameAction(false);

            _playerActionUtility.OnPlayerMove -= _gameManager.MovePlayerHandler;
            _playerActionUtility.OnRotateOrderLeft -= _gameManager.RotateOrderLeftHandler;
            _playerActionUtility.OnRotateOrderRight -= _gameManager.RotateOrderRightHandler;
        }
    }
}