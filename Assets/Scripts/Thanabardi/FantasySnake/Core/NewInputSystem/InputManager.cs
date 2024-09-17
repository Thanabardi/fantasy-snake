using Thanabardi.Generic.Utility;

namespace Thanabardi.FantasySnake.Core.NewInputSystem
{
    public class InputManager : MonoSingleton<InputManager>
    {
        public InputMaster InputMaster { get; private set; }

        public override void Awake()
        {
            base.Awake();
            InputMaster = new();
            InputMaster.Shortcut.Enable();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            InputMaster.Shortcut.Disable();
        }
    }
}