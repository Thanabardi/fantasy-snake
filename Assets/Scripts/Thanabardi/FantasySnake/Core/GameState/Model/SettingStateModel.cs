using Thanabardi.FantasySnake.Core.NewInputSystem;
using Thanabardi.FantasySnake.Core.GameSystem;
using Thanabardi.FantasySnake.Core.UI;
using Thanabardi.Generic.Core.StateSystem;

namespace Thanabardi.FantasySnake.Core.GameState.Model
{
    public class SettingStateModel : StateModel
    {
        private SettingPanel _settingPanel;

        public SettingStateModel() : base((int)GameStates.State.Setting, nameof(SettingStateModel)) { }

        public override void OnStateIn()
        {
            base.OnStateIn();
            _settingPanel = (SettingPanel)UIManager.Instance.SetPanelActive(UIManager.UIKey.SettingPanel, true);

            _settingPanel.OnExitButtonClicked += OnExitHandler;
            _settingPanel.OnMenuButtonClicked += OnMenuHandler;

            InputManager.Instance.InputMaster.Shortcut.Setting.performed += OnSettingShortcutPerformed;

            SoundManager.Instance.PlaySound(SoundManager.Instance.SettingMusic);
        }

        public override void OnStateOut()
        {
            base.OnStateOut();
            UIManager.Instance.SetPanelActive(UIManager.UIKey.SettingPanel, false);

            _settingPanel.OnExitButtonClicked -= OnExitHandler;
            _settingPanel.OnMenuButtonClicked -= OnMenuHandler;

            InputManager.Instance.InputMaster.Shortcut.Setting.performed -= OnSettingShortcutPerformed;

            SoundManager.Instance.StopSound(SoundManager.Instance.SettingMusic);
        }

        private void OnMenuHandler()
        {
            GameStateManager.Instance.GoToState((int)GameStates.State.Menu);
        }

        private void OnExitHandler()
        {
            GameStateManager.Instance.GoToState(GameStateManager.Instance.PreviousState.StateID);
        }

        private void OnSettingShortcutPerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            OnExitHandler();
        }
    }
}

