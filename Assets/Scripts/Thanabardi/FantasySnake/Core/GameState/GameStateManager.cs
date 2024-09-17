using Thanabardi.Generic.Core.StateSystem;

namespace Thanabardi.FantasySnake.Core.GameState
{
    public class GameStateManager : StateManager<GameStateManager>
    {
        public override void Awake()
        {
            base.Awake();
            GameStates gameStates = new();
            Initialize((int)GameStates.State.Menu, gameStates.GetGameStateModels());
        }
    }
}