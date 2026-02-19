using UnityEngine;

public class EnemyIdleState : EnemyState
{
    protected override void Action()
    {
        _enemyAI.agent.isStopped = true;
        _enemyAI.animator.SetBool("IsMoving", false);
    }

    protected override void Decision()
    {
        float distance = _enemyAI.DistanceToTarget();

        if(distance <= _enemyAI.pursueRange)
        {
            _enemyAI.Transition(State.Pursue);
        }
    }
}
