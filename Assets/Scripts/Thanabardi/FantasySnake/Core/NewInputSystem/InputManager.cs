using UnityEngine.InputSystem;
using System;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using System.Linq;
using Thanabardi.Generic.Utility;

namespace Thanabardi.FantasySnake.Core.NewInputSystem
{
    public class InputManager : MonoSingleton<InputManager>
    {
        public InputMaster InputMaster { get; private set; }
        public InputDevice CurrentInputDevice { get; private set; }
        public event Action OnSwitchInput;

        private const float _INPUT_MAGNITUDE_THRESHOLD = 0.0001f;
        private const string _KEYBOARD_MOUSE_SCHEME = "KeyboardMouse";
        private const string _GAMRPAD_SCHEME = "Gamepad";

        public override void Awake()
        {
            base.Awake();
            InputSystem.onEvent += OnInputSystemEvent;
            InputMaster = new();
            InputMaster.Shortcut.Enable();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            InputSystem.onEvent -= OnInputSystemEvent;
            InputMaster.Shortcut.Disable();
        }

        public string GetKeyName(InputAction inputAction, int bindingIndex = 0)
        {
            try
            {
                switch (CurrentInputDevice)
                {
                    case Mouse:
                    case Keyboard:
                    default:
                        bindingIndex += inputAction.GetBindingIndex(InputBinding.MaskByGroup(_KEYBOARD_MOUSE_SCHEME));
                        return inputAction.bindings[bindingIndex].ToDisplayString(InputBinding.DisplayStringOptions.DontIncludeInteractions);
                    case Gamepad:
                        bindingIndex += inputAction.GetBindingIndex(InputBinding.MaskByGroup(_GAMRPAD_SCHEME));
                        return inputAction.bindings[bindingIndex].ToDisplayString(InputBinding.DisplayStringOptions.DontUseShortDisplayNames);
                }
            }
            catch (ArgumentOutOfRangeException argumentOutOfRangeException)
            {
                Debug.LogWarning($"Doesn't have binding key for {inputAction.name} with {CurrentInputDevice.name} Error: {argumentOutOfRangeException.Message}");
                return null;
            }
        }

        public void EnableGameAction(bool isEnter)
        {
            InputMaster ??= new();
            if (isEnter)
            {
                InputMaster.Gameplay.Enable();
            }
            else
            {
                InputMaster.Gameplay.Disable();
            }
        }

        private void OnInputSystemEvent(InputEventPtr eventPtr, InputDevice device)
        {
            if (CurrentInputDevice == device ||
                (CurrentInputDevice is Keyboard && device is Mouse) ||
                (CurrentInputDevice is Mouse && device is Keyboard))
            {
                return;
            }

            // filter noisy input mainly from controller.
            var eventType = eventPtr.type;
            if (eventType == StateEvent.Type)
            {
                // find is there a changed control that have value above magnitudeThreshold
                if (!eventPtr.EnumerateChangedControls(device, _INPUT_MAGNITUDE_THRESHOLD).Any())
                {
                    return;
                }
            }
            CurrentInputDevice = device;
            OnSwitchInput?.Invoke();
        }
    }
}