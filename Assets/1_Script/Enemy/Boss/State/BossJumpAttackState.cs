using UnityEngine;
using UnityEngine.UIElements;

public class BossJumpAttackState : IBossState
{
    private BossController boss;
    private BossStateMachine stateMachine;

    private float timer;
    private float duration = 1f;

    private Vector3 startPos;
    private Vector3 targetPos;

    private float jumpHeight = 3f;

    public BossJumpAttackState(BossController boss, BossStateMachine stateMachine)
    {
        this.boss = boss;
        this.stateMachine = stateMachine;
    }


    public void Enter()
    {
        boss.agent.isStopped = true;
        boss.agent.enabled = false;

        startPos = boss.transform.position;
        targetPos = boss.player.position;

        boss.animator.Play("JumpAttack");
        timer = 0f;
    }

    public void Update()
    {
        timer += Time.deltaTime;

        boss.transform.position = Vector3.Lerp(
            boss.transform.position,
            boss.player.position,
            Time.deltaTime * 2.2f
        );

        if (timer >= duration)
        {
            boss.agent.enabled = true;
            stateMachine.ChangeState(boss.chaseState);
        }
    }

    public void Exit()
    {
        boss.agent.enabled = true;
        boss.agent.Warp(boss.transform.position);
        boss.agent.isStopped = false;
    }
}
