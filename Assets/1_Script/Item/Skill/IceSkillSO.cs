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

    public GameObject icePrefab; // 떨어지는 얼음
    public GameObject hitVFX;

    public LayerMask enemyLayer;

    public override void UseSkill(GameObject user)
    {
        // 떨어질 위치 계산 (플레이어 앞)
        Vector3 targetPos = user.transform.position + user.transform.forward * forwardOffset;

        // 하늘 위치
        Vector3 spawnPos = targetPos + Vector3.up * dropHeight;

        // 얼음 생성
        if (icePrefab != null)
        {
            GameObject ice = Instantiate(icePrefab, spawnPos, Quaternion.identity);

            // 떨어지는 로직 실행
            MonoBehaviour runner = user.GetComponent<InputHandler>();
            runner.StartCoroutine(DropIce(ice, targetPos));
        }
    }

    private System.Collections.IEnumerator DropIce(GameObject ice, Vector3 targetPos)
    {
        float speed = 20f;

        while (ice != null && Vector3.Distance(ice.transform.position, targetPos) > 0.2f)
        {
            ice.transform.position = Vector3.MoveTowards(
                ice.transform.position,
                targetPos,
                speed * Time.deltaTime
            );

            yield return null;
        }

        // 도착하면 폭발
        Explode(targetPos);

        if (ice != null)
            GameObject.Destroy(ice);
    }

    private void Explode(Vector3 position)
    {
        if (hitVFX != null)
            Instantiate(hitVFX, position, Quaternion.identity);

        Collider[] enemies = Physics.OverlapSphere(position, radius, enemyLayer);

        Debug.Log($"IceSkill 적 {enemies.Length}명 감지");

        foreach (var enemy in enemies)
        {
            IDamageable damageable = enemy.GetComponent<IDamageable>();

            if (damageable != null)
            {
                damageable.TakeDamage(damage);

                // 슬로우 적용
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
