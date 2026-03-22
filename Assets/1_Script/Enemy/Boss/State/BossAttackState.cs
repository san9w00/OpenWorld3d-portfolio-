using UnityEngine;

public class BossAttackState : IBossState
{
    private BossController boss;
    private BossStateMachine stateMachine;

    private float attackTimer;
    private float attackDuration = 2f;

    public BossAttackState(BossController boss, BossStateMachine stateMachine)
    {
        this.boss = boss;
        this.stateMachine = stateMachine;
    }

    public void Enter()
    {
        int rand = Random.Range(0, 2);

        if (rand == 0)
            boss.animator.Play("Attack1");
        else
            boss.animator.Play("Attack2");

        attackTimer = 0f;
    }

    public void Update()
    {
        attackTimer += Time.deltaTime;

        float dist = boss.DistanceToPlayer();

        if (dist >= boss.attackRange)
        {
            stateMachine.ChangeState(boss.chaseState);
            return;
        }

        if (attackTimer >= attackDuration)
        {
            stateMachine.ChangeState(boss.chaseState);
        }
    }

    public void Exit()
    {
    }
}
