using UnityEngine;

public class BossIdleState : IBossState
{
    private BossController boss;
    private BossStateMachine stateMachine;

    public BossIdleState(BossController boss, BossStateMachine stateMachine)
    {
        this.boss = boss;
        this.stateMachine = stateMachine;
    }

    public void Enter()
    {
        boss.animator.Play("Idle");
    }

    public void Update()
    {
        float dist = boss.DistanceToPlayer();

        if (dist <= boss.detectRange)
        {
            stateMachine.ChangeState(boss.chaseState);
        }

    }

    public void Exit()
    {
    }
}
