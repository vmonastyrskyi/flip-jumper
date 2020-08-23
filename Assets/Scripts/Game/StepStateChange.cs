using UnityEngine;

namespace Game
{
    public class StepStateChange : StateMachineBehaviour
    {
        private static readonly int IsStepOn = Animator.StringToHash("IsStepOn");

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool(IsStepOn, false);
        }
    }
}
