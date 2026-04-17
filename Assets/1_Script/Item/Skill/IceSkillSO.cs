using UnityEngine;

[CreateAssetMenu(menuName = "Items/Skills/IceSkill")]
public class IceSkillSO : WeaponSkillSO
{
    [Header("Damage")]
    public float damage = 130f;

    [Header("Slow")]
    public float slowPercent = 0.5f; // 50% 감소
    public float slowDuration = 5f;

    [Header("Range")]
    public float radius = 3f;

    [Header("Ice Drop")]
    public float dropHeight = 10f; // 위에서 떨어지는 높이
    public float forwardOffset = 5f; // 플레이어 앞 거리

    public LayerMask enemyLayer;

    public override void UseSkill(GameObject user)
    {
        // 떨어질 위치 계산 (플레이어 앞)
        Vector3 targetPos = user.transform.position + user.transform.forward * forwardOffset;

        EventBus.Publish(new VFXEvent(
             targetPos,
             VFXActionType.Skill,
             VFXSwordType.Ice
        ));

        Collider[] enemies = Physics.OverlapSphere(targetPos, radius, enemyLayer);

        Debug.Log($"IceSkill 적 {enemies.Length}명 감지");

        foreach (var enemy in enemies)
        {
            IDamageable damageable = enemy.GetComponent<IDamageable>();

            if (damageable != null)
            {
                damageable.TakeDamage(damage);

                if (enemy.gameObject.activeInHierarchy)
                {
                    MonoBehaviour runner = enemy.GetComponent<MonoBehaviour>();
                    runner.StartCoroutine(ApplySlow(enemy));
                }
            }
        }
    }

    private System.Collections.IEnumerator ApplySlow (Collider enemy)
    {
        EnemyAI ai = enemy.GetComponent<EnemyAI>();

        if (ai == null) yield break;

        float originalSpeed = ai.agent.speed;

        ai.agent.speed *= slowPercent;

        yield return new WaitForSeconds(slowDuration);

        if (ai != null && ai.gameObject.activeInHierarchy)
        {
            ai.agent.speed = originalSpeed;
        }
    }

#if UNITY_EDITOR
    public override void DrawGizmo(Vector3 position)
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(position, radius);
    }
#endif
}
