using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Skill/Turtle Defense")]
public class TurtleDefenseSkillSO : EnemySkillSO
{
    [Range(0f, 1f)]
    public float damageMultiplier = 0.3f;

    public override void Use(EnemyAI enemyAI, EnemyStatus status)
    {
        enemyAI.StartCoroutine(DefenseRoutine(status));
    }

    private IEnumerator DefenseRoutine(EnemyStatus status)
    {
        Debug.Log("방어 시작");

        status.SetDamageMultiplier(damageMultiplier);

        yield return new WaitForSeconds(duration);

        status.SetDamageMultiplier(1f);

        Debug.Log("방어 종료");
    }
}
