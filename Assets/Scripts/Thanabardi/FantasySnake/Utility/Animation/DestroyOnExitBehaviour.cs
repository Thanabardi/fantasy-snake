using UnityEngine;

namespace Thanabardi.FantasySnake.Utility.Animation
{
    public class DestroyOnExitBehaviour : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Destroy(animator.gameObject, stateInfo.length);
        }
    }
}