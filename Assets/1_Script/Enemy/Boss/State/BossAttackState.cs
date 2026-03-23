using UnityEngine;
using UnityEngine.AI;

public class BossAttackState : IBossState
{
    private BossController boss;
    private BossStateMachine stateMachine;

    private float attackTimer;
    private float attackDuration = 2.7f;

    public BossAttackState(BossController boss, BossStateMachine stateMachine)
    {
        this.boss = boss;
        this.stateMachine = stateMachine;
    }

    public void Enter()
    {
        boss.agent.isStopped = true;
        boss.agent.ResetPath();
        boss.agent.velocity = Vector3.zero;

        boss.agent.updatePosition = false;

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

        Vector3 dir = boss.player.position - boss.transform.position;
        float dist = boss.DistanceToPlayer();      

        if (dir != Vector3.zero)
        {
            boss.transform.rotation = Quaternion.Slerp(boss.transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 10f);
        }

        if (attackTimer >= attackDuration)
        {
            stateMachine.ChangeState(boss.chaseState);
        }
    }

    public void Exit()
    {
        boss.agent.isStopped = false;
        boss.agent.updatePosition = true;
    }
}
