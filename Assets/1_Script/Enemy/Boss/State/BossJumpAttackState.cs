using UnityEngine;
using UnityEngine.UIElements;

public class BossJumpAttackState : IBossState
{
    private BossController boss;
    private BossStateMachine stateMachine;

    private float timer;
    private float duration = 2f;

    public BossJumpAttackState(BossController boss, BossStateMachine stateMachine)
    {
        this.boss = boss;
        this.stateMachine = stateMachine;
    }


    public void Enter()
    {
        boss.agent.enabled = false;
        boss.animator.Play("JumpAttack");
        timer = 0f;
    }

    public void Update()
    {
        timer += Time.deltaTime;

        boss.transform.position = Vector3.Lerp(
            boss.transform.position,
            boss.player.position,
            Time.deltaTime * 5f
        );

        if (timer >= duration)
        {
            boss.agent.enabled = true;
            stateMachine.ChangeState(boss.chaseState);
        }
    }

    public void Exit()
    {
    }
}
