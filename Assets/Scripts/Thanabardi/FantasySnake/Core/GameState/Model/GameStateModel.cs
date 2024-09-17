using UnityEngine;
using UnityEngine.InputSystem;
using Thanabardi.FantasySnake.Core.GameScene;
using Thanabardi.FantasySnake.Core.NewInputSystem;
using Thanabardi.FantasySnake.Core.GameSystem;
using Thanabardi.FantasySnake.Utility;
using Thanabardi.Generic.Core.StateSystem;

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
                // don't switch scene when change game state from setting to game
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
            InputManager.Instance.InputMaster.Gameplay.Disable();

            InputManager.Instance.InputMaster.Shortcut.Setting.performed -= OnSettingShortcutPerformed;

            _playerActionUtility.OnPlayerMove -= _gameManager.MovePlayerHandler;
            _playerActionUtility.OnRotateOrderLeft -= _gameManager.RotateOrderLeftHandler;
            _playerActionUtility.OnRotateOrderRight -= _gameManager.RotateOrderRightHandler;

            SoundManager.Instance.StopSound(SoundManager.Instance.GameplayMusic);
            SoundManager.Instance.StopSound(SoundManager.Instance.CaveAMB);
        }

        private void CallOnStateIn()
        {
            InputManager.Instance.InputMaster.Gameplay.Enable();
            _gameManager = GameObject.FindObjectOfType<GameManager>();
            _playerActionUtility = GameObject.FindObjectOfType<PlayerActionUtility>();

            InputManager.Instance.InputMaster.Shortcut.Setting.performed += OnSettingShortcutPerformed;

            _playerActionUtility.OnPlayerMove += _gameManager.MovePlayerHandler;
            _playerActionUtility.OnRotateOrderLeft += _gameManager.RotateOrderLeftHandler;
            _playerActionUtility.OnRotateOrderRight += _gameManager.RotateOrderRightHandler;

            SoundManager.Instance.PlaySound(SoundManager.Instance.GameplayMusic);
            SoundManager.Instance.PlaySound(SoundManager.Instance.CaveAMB);
        }

        private void OnSettingShortcutPerformed(InputAction.CallbackContext context)
        {
            GameStateManager.Instance.GoToState((int)GameStates.State.Setting);
        }
    }
}

