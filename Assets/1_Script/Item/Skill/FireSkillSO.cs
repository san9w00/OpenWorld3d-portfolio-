using UnityEngine;

[CreateAssetMenu(menuName = "Items/Skills/FireSkill")]
public class FireSkillSO : WeaponSkillSO
{
    public float radius = 4f; 
    public float tickDamage = 7f;
    public float duration = 4f;
    public float tickInterval = 1f;

    public LayerMask enemyLayer;

    public override void UseSkill(GameObject user)
    {
        EventBus.Publish(new VFXEvent(
            user.transform.position,
            VFXActionType.Skill,
            VFXSwordType.Fire
            ));

        Collider[] enemies = Physics.OverlapSphere(user.transform.position, radius, enemyLayer);

        Debug.Log($"FireSkill 적 {enemies.Length}명 감지");

        foreach (var enemy in enemies)
        {
            IDamageable damageable = enemy.GetComponent<IDamageable>();

            if (damageable != null)
            {
                MonoBehaviour runner = user.GetComponent<InputHandler>();
                runner.StartCoroutine(BurnCoroutine(damageable));
            }
        }
    }

    private System.Collections.IEnumerator BurnCoroutine(IDamageable target)
    {
        float elapsed = 0f;

        MonoBehaviour mb = target as MonoBehaviour;

        while (elapsed < duration)
        {
            if (mb == null || !mb.gameObject.activeInHierarchy)
                yield break;

            target.TakeDamage(tickDamage);

            if (mb != null)
            {
                EventBus.Publish(new VFXEvent(
                    mb.transform.position + Vector3.up * 1f,
                    VFXActionType.Skill_Second,
                    VFXSwordType.Fire
                ));
            }

            yield return new WaitForSeconds(tickInterval);
            elapsed += tickInterval;
        }
    }

#if UNITY_EDITOR
    public override void DrawGizmo(Vector3 position)
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(position, radius);
    }
#endif
}
