using Thanabardi.FantasySnake.Core.System;
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
        }

        public override void OnStateOut()
        {
            base.OnStateOut();
            UIManager.Instance.SetPanelActive(UIManager.UIKey.MenuPanel, false);
        }
    }
}