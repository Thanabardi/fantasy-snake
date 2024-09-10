using Thanabardi.FantasySnake.Core.System;
using Thanabardi.FantasySnake.Core.UI;
using Thanabardi.Generic.Core.StateSystem;
using UnityEngine;

namespace Thanabardi.FantasySnake.Core.GameState.Model
{
    public class MenuStateModel : StateModel
    {
        private MenuPanel _menuPanel;
        public MenuStateModel() : base((int)GameStates.State.Menu, nameof(MenuStateModel)) { }
        public override void OnStateIn()
        {
            base.OnStateIn();
            _menuPanel = (MenuPanel)UIManager.Instance.SetPanelActive(UIManager.UIKey.MenuPanel, true);
            _menuPanel.OnPlayButtonClicked += PlayGame;
            _menuPanel.OnSettingButtonClicked += Setting;
            _menuPanel.OnExitButtonClicked += ExitGame;
        }

        public override void OnStateOut()
        {
            base.OnStateOut();
            UIManager.Instance.SetPanelActive(UIManager.UIKey.MenuPanel, false);
            _menuPanel.OnPlayButtonClicked -= PlayGame;
            _menuPanel.OnSettingButtonClicked -= Setting;
            _menuPanel.OnExitButtonClicked -= ExitGame;
        }

        private void PlayGame()
        {
            GameStateManager.Instance.GoToState((int)GameStates.State.GamePlay);
        }

        private void Setting()
        {
            GameStateManager.Instance.GoToState((int)GameStates.State.Setting);
        }

        private void ExitGame()
        {
            Application.Quit();
        }
    }
}