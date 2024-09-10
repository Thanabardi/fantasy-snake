using Thanabardi.FantasySnake.Core.NewInputSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Thanabardi.FantasySnake.Core.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        private InputMaster.GameplayActions _gameplayActions;
        private void Awake()
        {
            _gameplayActions = InputManager.Instance.InputMaster.Gameplay;
        }

        private void OnEnable()
        {
            _gameplayActions.MoveUp.performed += MoveUp;
            _gameplayActions.MoveDown.performed += MoveDown;
            _gameplayActions.MoveLeft.performed += MoveLeft;
            _gameplayActions.MoveRight.performed += MoveRight;
        }
        private void OnDisable()
        {
            _gameplayActions.MoveUp.performed -= MoveUp;
            _gameplayActions.MoveDown.performed -= MoveDown;
            _gameplayActions.MoveLeft.performed -= MoveLeft;
            _gameplayActions.MoveRight.performed -= MoveRight;
        }

        private void MoveUp(InputAction.CallbackContext context)
        {
            transform.position += Vector3.forward;
        }
        private void MoveDown(InputAction.CallbackContext context)
        {
            transform.position += Vector3.back;
        }
        private void MoveLeft(InputAction.CallbackContext context)
        {
            transform.position += Vector3.left;
        }
        private void MoveRight(InputAction.CallbackContext context)
        {
            transform.position += Vector3.right;
        }
    }
}