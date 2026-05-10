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
            if (attack.attackType == BossAttackType.TeleportBite)
            {
                yield return StartCoroutine(TeleportBite(attack));
            }
            else if (attack.attackType == BossAttackType.FireBreath)
            {
                yield return StartCoroutine(FireBreath(attack));
            }
            else if (attack.attackType == BossAttackType.Stun)
            {
                yield return StartCoroutine(StunState(attack));
            }
            else
            {
                // 데미지 설정 (기본공격 할퀴기)
                boss.currentAttackDamage = boss.GetFinalDamage(attack.damage);

                // 애니메이션 실행
                _enemyAI.animator.SetTrigger(attack.animationTrigger);

                // 공격 끝날때까지 대기
                yield return new WaitForSeconds(attack.duration);
            }
        }

        yield return new WaitForSeconds(2f);
        isRunningPattern = false;

        CheckStateAfterPattern(); // 상태 체크 (플레이어 어디??)
    }

    private IEnumerator TeleportBite(BossAttackSO attack)
    {
        Transform player = _enemyAI.target;

        // 사라지기
        boss.currentAttackDamage = 0;
        boss.bossModel.SetActive(false);

        // 사라진 자리에 광역 데미지
        DoAOEDamage(transform.position, attack.aoeRadius, attack.aoeDamage);
        EventBus.Publish(new VFXEvent(transform.position, VFXActionType.BossAOE, VFXSwordType.None));

        // 대기하기
        yield return new WaitForSeconds(attack.teleportDelay);

        // 플레이어 앞 위치 계산
        Vector3 dir = player.forward;
        Vector3 targetPos = player.position + dir * 2f;

        transform.position = targetPos;

        // 플레이어 보기
        Vector3 lookDir = (player.position - transform.position).normalized;
        lookDir.y = 0;
        transform.forward = lookDir;

        // 등장
        boss.bossModel.SetActive(true);

        // 공격 데미지 다시 설정
        boss.currentAttackDamage = boss.GetFinalDamage(attack.damage);

        // 애니메이션
        _enemyAI.animator.SetTrigger(attack.animationTrigger);

        yield return new WaitForSeconds(attack.duration);
    }

    private void DoAOEDamage(Vector3 center, float radius, float damage)
    {
        Collider[] hits = Physics.OverlapSphere(center, radius);

        foreach (var hit in hits)
        {
            IDamageable damageable = hit.GetComponent<IDamageable>();

            if (damageable != null)
            {
                damageable.TakeDamage(damage);
            }
        }
    }

    private IEnumerator FireBreath(BossAttackSO attack)
    {
        Vector3 dir = (_enemyAI.target.position - transform.position).normalized;
        dir.y = 0;
        transform.forward = dir;

        // 이동 멈춤
        _enemyAI.agent.isStopped = true;

        // 애니메이션
        _enemyAI.animator.SetTrigger(attack.animationTrigger);

        yield return new WaitForSeconds(1f);

        // 데미지 설정
        boss.currentAttackDamage = boss.GetFinalDamage(attack.damage);

        // 브레스 On
        boss.fireBreathObject.SetActive(true);

        // 3초 유지
        yield return new WaitForSeconds(3f);

        // 브레스 Off
        boss.fireBreathObject.SetActive(false);
    }

    private IEnumerator StunState(BossAttackSO attack)
    {
        // 이동 완전 정지
        _enemyAI.agent.isStopped = true;
        _enemyAI.agent.ResetPath();

        // 기절 애니메이션
        _enemyAI.animator.SetTrigger(attack.animationTrigger);

        // 5초 기절
        yield return new WaitForSeconds(attack.duration);
    }

    private void CheckStateAfterPattern()
    {
        float distance = _enemyAI.DistanceToTarget();

        if (distance > _enemyAI.attackRange)
        {
            _enemyAI.Transition(State.Pursue);
        }
    }

    private void LookAtTarget()
    {
        Vector3 dir = (_enemyAI.target.position - transform.position).normalized;
        dir.y = 0;
        transform.forward = dir;
    }
}
