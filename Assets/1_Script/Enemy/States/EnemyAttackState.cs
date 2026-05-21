using System.Collections;
using System.Threading;
using UnityEngine;

public class EnemyAttackState : EnemyState
{
    private float timer;
    private bool firstAttackDone;

    private bool isUsingSkill;

    private void OnEnable()
    {
        timer = 0;
        firstAttackDone = false;
    }

    protected override void Action()
    {
        if (isUsingSkill)
        {
            _enemyAI.agent.isStopped = true;
            _enemyAI.agent.ResetPath();
            _enemyAI.animator.SetBool("IsMoving", false);
            return;
        }

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
        if (isUsingSkill)
            return;

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
        float roll = Random.Range(0f, 100f);

        bool canUseSkill = _enemyStatus.Data.skills != null && _enemyStatus.Data.skills.Length > 0;

        if (canUseSkill && roll <= _enemyStatus.Data.skillChance)
        {
            UseSkill();
        }
        else
        {
            NormalAttack();
        }
    }

    private void UseSkill()
    {
        EnemySkillSO skill = _enemyStatus.Data.skills[Random.Range(0, _enemyStatus.Data.skills.Length)];

        StartCoroutine(SkillRoutine(skill));
    }

    private IEnumerator SkillRoutine(EnemySkillSO skill)
    {
        isUsingSkill = true;

        _enemyAI.animator.SetTrigger(skill.animationTrigger);

        skill.Use(_enemyAI, _enemyStatus);

        yield return new WaitForSeconds(skill.duration);

        _enemyAI.animator.ResetTrigger(skill.animationTrigger);
        _enemyAI.animator.Play("Idle");

        isUsingSkill = false;
    }

    private void NormalAttack()
    {
        Debug.Log("일반 공격!");
        _enemyAI.animator.SetTrigger("Attack");
    }
}
