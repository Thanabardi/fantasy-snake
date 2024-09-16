using Thanabardi.FantasySnake.Core.GameScene;
using Thanabardi.FantasySnake.Core.NewInputSystem;
using Thanabardi.FantasySnake.Core.System;
using Thanabardi.FantasySnake.Utility;
using Thanabardi.Generic.Core.StateSystem;
using UnityEngine;
using UnityEngine.InputSystem;

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
            if (GameStateManager.Instance.PreviousState.StateID == (int)GameStates.State.Setting)
            {
                CallOnStateIn();
            }
            else
            {
                GameSceneManager.Instance.GoToScene(GameSceneManager.SceneKey.GameScene, CallOnStateIn);
            }
        }

        public override void OnStateOut()
        {
            base.OnStateOut();
            InputManager.Instance.EnableGameAction(false);

            InputManager.Instance.InputMaster.Shortcut.Setting.performed -= OnSettingShortcutPerformed;

            _playerActionUtility.OnPlayerMove -= _gameManager.MovePlayerHandler;
            _playerActionUtility.OnRotateOrderLeft -= _gameManager.RotateOrderLeftHandler;
            _playerActionUtility.OnRotateOrderRight -= _gameManager.RotateOrderRightHandler;
            SoundManager.Instance.StopSound2D(SoundManager.Instance.GameplayMusic);
            SoundManager.Instance.StopSound2D(SoundManager.Instance.CaveAMB);
        }

        private void CallOnStateIn()
        {
            InputManager.Instance.EnableGameAction(true);
            _gameManager = GameObject.FindObjectOfType<GameManager>();
            _playerActionUtility = GameObject.FindObjectOfType<PlayerActionUtility>();

            InputManager.Instance.InputMaster.Shortcut.Setting.performed += OnSettingShortcutPerformed;

            _playerActionUtility.OnPlayerMove += _gameManager.MovePlayerHandler;
            _playerActionUtility.OnRotateOrderLeft += _gameManager.RotateOrderLeftHandler;
            _playerActionUtility.OnRotateOrderRight += _gameManager.RotateOrderRightHandler;
            SoundManager.Instance.PlaySound2D(SoundManager.Instance.GameplayMusic);
            SoundManager.Instance.PlaySound2D(SoundManager.Instance.CaveAMB);
        }

        private void OnSettingShortcutPerformed(InputAction.CallbackContext context)
        {
            GameStateManager.Instance.GoToState((int)GameStates.State.Setting);
        }
    }
}

