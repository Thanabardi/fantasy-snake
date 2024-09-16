using Thanabardi.FantasySnake.Core.GameState.Model;
using Thanabardi.Generic.Core.StateSystem;

namespace Thanabardi.FantasySnake.Core.GameState
{
    public class GameStates : States
    {
        private StateModel[] _states;

        public override StateModel[] GetGameStateModels()
        {
            _states ??= new StateModel[]
            {
                new MenuStateModel(),
                new GameStateModel(),
            };
            return _states;
        }

        public enum State
        {
            Menu,
            GamePlay,
        }
    }
}