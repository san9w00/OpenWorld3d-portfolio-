using UnityEngine;

public class BossChaseState : IBossState
{
    private BossController boss;
    private BossStateMachine stateMachine;

    public BossChaseState(BossController boss, BossStateMachine stateMachine)
    {
        this.boss = boss;
        this.stateMachine = stateMachine;
    }

    public void Enter()
    {
        boss.animator.Play("Run");
    }

    public void Update()
    { 
        float dist = boss.DistanceToPlayer();

        boss.agent.SetDestination(boss.player.position);

        if (dist <= boss.attackRange)
        {
            stateMachine.ChangeState(boss.attackState);
        }
        else if (dist <= boss.jumpAttackRange)
        {
            stateMachine.ChangeState(boss.jumpAttackState);
        }
    }

    public void Exit()
    {
        boss.agent.ResetPath();
    }   
}
