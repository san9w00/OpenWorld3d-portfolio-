using UnityEngine;
using UnityEngine.UIElements;

public class BossJumpAttackState : IBossState
{
    private BossController boss;
    private BossStateMachine stateMachine;

    private float timer;
    private float duration = 1.6f;

    private Vector3 startPos;
    private Vector3 targetPos;

    private float jumpHeight = 3.5f;

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

        float t = timer / duration;

        Vector3 currentPos = Vector3.Lerp(startPos, targetPos, t);

        float height = Mathf.Sin(t * Mathf.PI) * jumpHeight;

        currentPos.y += height;

        boss.transform.position = currentPos;

        if (timer >= duration)
        {
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
