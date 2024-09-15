using System;
using Thanabardi.FantasySnake.Core.NewInputSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Thanabardi.FantasySnake.Utility
{
    public class PlayerActionUtility : MonoBehaviour
    {
        public event Action<Vector2> OnPlayerMove;
        public event Action OnRotateOrderLeft;
        public event Action OnRotateOrderRight;

        private void OnEnable()
        {
            InputManager.Instance.InputMaster.Gameplay.MoveUp.performed += MoveUp;
            InputManager.Instance.InputMaster.Gameplay.MoveDown.performed += MoveDown;
            InputManager.Instance.InputMaster.Gameplay.MoveLeft.performed += MoveLeft;
            InputManager.Instance.InputMaster.Gameplay.MoveRight.performed += MoveRight;
            InputManager.Instance.InputMaster.Gameplay.RotateOrderLeft.performed += RotateOrderLeftHandler;
            InputManager.Instance.InputMaster.Gameplay.RotateOrderRight.performed += RotateOrderRightHandler;
        }
        private void OnDisable()
        {
            InputManager.Instance.InputMaster.Gameplay.MoveUp.performed -= MoveUp;
            InputManager.Instance.InputMaster.Gameplay.MoveDown.performed -= MoveDown;
            InputManager.Instance.InputMaster.Gameplay.MoveLeft.performed -= MoveLeft;
            InputManager.Instance.InputMaster.Gameplay.MoveRight.performed -= MoveRight;
            InputManager.Instance.InputMaster.Gameplay.RotateOrderLeft.performed += RotateOrderLeftHandler;
            InputManager.Instance.InputMaster.Gameplay.RotateOrderRight.performed += RotateOrderRightHandler;
        }

        private void MoveUp(InputAction.CallbackContext context)
        {
            Move(Vector2.up);
        }

        private void MoveDown(InputAction.CallbackContext context)
        {
            Move(Vector2.down);
        }

        private void MoveLeft(InputAction.CallbackContext context)
        {
            Move(Vector2.left);
        }

        private void MoveRight(InputAction.CallbackContext context)
        {
            Move(Vector2.right);
        }

        private void RotateOrderLeftHandler(InputAction.CallbackContext context)
        {
            OnRotateOrderLeft?.Invoke();
        }

        private void RotateOrderRightHandler(InputAction.CallbackContext context)
        {
            OnRotateOrderRight?.Invoke();
        }

        private void Move(Vector2 direction)
        {
            OnPlayerMove?.Invoke(direction);
        }
    }
}