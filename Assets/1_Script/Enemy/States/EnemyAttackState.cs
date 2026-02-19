using System.Threading;
using UnityEngine;

public class EnemyAttackState : EnemyState
{
    private float timer;
    private bool firstAttackDone;

    private void OnEnable()
    {
        timer = 0;
        firstAttackDone = false;
    }

    protected override void Action()
    {
        _enemyAI.agent.isStopped = true;
        _enemyAI.agent.ResetPath();
        _enemyAI.animator.SetBool("IsMoving", false);

        // 플레이어 바라보기
        Vector3 dir = (_enemyAI.target.position - transform.position).normalized;
        dir.y = 0;
        transform.forward = dir;

        if (!firstAttackDone)
        {
            Attack();
            firstAttackDone = true;
            return;
        }

        timer += Time.deltaTime;

        if(timer >= _enemyStatus.Data.atkCooldown)
        {
            Attack();
            timer = 0;
        }
    }

    protected override void Decision()
    {
        float distance = _enemyAI.DistanceToTarget();

        if(distance > _enemyAI.attackRange && distance <= _enemyAI.pursueRange)
        {
            _enemyAI.Transition(State.Pursue);
        }
        else if (distance > _enemyAI.pursueRange)
        {
            _enemyAI.Transition(State.Idle);
        }
    }

    private void Attack()
    {
        Debug.Log("적 공격!");
        _enemyAI.animator.SetTrigger("Attack");
    }
}
