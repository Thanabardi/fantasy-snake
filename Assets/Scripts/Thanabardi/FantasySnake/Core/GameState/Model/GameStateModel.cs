using Thanabardi.FantasySnake.Core.GameScene;
using Thanabardi.Generic.Core.StateSystem;

namespace Thanabardi.FantasySnake.Core.GameState.Model
{
    public class GameStateModel : StateModel
    {
        public GameStateModel() : base((int)GameStates.State.GamePlay, nameof(GameStateModel)) { }
        public override void OnStateIn()
        {
            base.OnStateIn();
            GameSceneManager.Instance.GoToScene(GameSceneManager.SceneKey.GameScene);
        }

        public override void OnStateOut()
        {
            base.OnStateOut();
        }
    }
}