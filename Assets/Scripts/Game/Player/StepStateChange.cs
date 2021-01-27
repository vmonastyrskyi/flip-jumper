using UnityEngine;

namespace Game.Player
{
    public class StepStateChange : StateMachineBehaviour
    {
        private static readonly int IsStepped = Animator.StringToHash("IsStepped");

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool(IsStepped, false);
        }
    }
}