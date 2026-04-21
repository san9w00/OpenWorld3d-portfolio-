using System.Collections;
using UnityEngine;

public class BossAttackState : EnemyState
{
    private BossAI boss;
    private bool isRunningPattern;

    protected override void Awake()
    {
        base.Awake();
        boss = GetComponent<BossAI>();
    }

    protected override void Action()
    {
        _enemyAI.agent.isStopped = true;
        _enemyAI.animator.SetBool("IsMoving", false);

        LookAtTarget();

        if (!isRunningPattern)
        {
            StartCoroutine(RunPattern());
        }
    }

    protected override void Decision()
    {
        if (isRunningPattern) return;

        float distance = _enemyAI.DistanceToTarget();

        if (distance > _enemyAI.attackRange)
        {
            _enemyAI.Transition(State.Pursue);
        }
    }

    private IEnumerator RunPattern()
    {
        isRunningPattern = true;

        // 랜덤 패턴 선택
        BossPatternSO pattern = boss.GetRandomPattern();

        foreach (var attack in pattern.attacks)
        {
            // 데미지 설정
            boss.currentAttackDamage = boss.GetFinalDamage(attack.damage);

            // 애니메이션 실행
            _enemyAI.animator.SetTrigger(attack.animationTrigger);

            // 공격 끝날때까지 대기
            yield return new WaitForSeconds(attack.duration);
        }

        isRunningPattern = false;
    }

    private void LookAtTarget()
    {
        Vector3 dir = (_enemyAI.target.position - transform.position).normalized;
        dir.y = 0;
        transform.forward = dir;
    }
}
