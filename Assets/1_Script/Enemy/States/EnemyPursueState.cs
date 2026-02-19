using UnityEngine;

public class EnemyPursueState : EnemyState
{
    protected override void Action()
    {
        _enemyAI.agent.isStopped = false;
        _enemyAI.agent.SetDestination(_enemyAI.target.position);

        _enemyAI.animator.SetBool("IsMoving", true);
    }

    protected override void Decision()
    {
        float distance = _enemyAI.DistanceToTarget();

        if(distance <= _enemyAI.attackRange)
        {
            _enemyAI.Transition(State.Attack);
        }
        else if (distance > _enemyAI.pursueRange)
        {
            _enemyAI.Transition(State.Idle);
        }
    }
}
