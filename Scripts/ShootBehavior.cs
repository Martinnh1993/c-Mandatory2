using UnityEngine;
public class ShootBehavior : StateMachineBehaviour
{
    // OnStateEnter is called when the shoot animation state is entered
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Player player = animator.gameObject.GetComponent<Player>();
        if (player != null)
        {
            player.AttemptShoot(); // This may play the shoot animation and sound
        }
    }

    // OnStateExit is called when the shoot animation state is exited
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Player player = animator.gameObject.GetComponent<Player>();
        if (player != null)
        {
            player.OnShootEnd(); // This would apply the force to the ball
        }
    }
}
