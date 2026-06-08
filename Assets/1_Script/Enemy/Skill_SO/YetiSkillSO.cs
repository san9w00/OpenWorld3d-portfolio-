using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Skill/Ice Field")]
public class YetiSkillSO : EnemySkillSO
{
    [SerializeField] private GameObject iceFieldPrefab;

    [SerializeField] private float spawnDistance = 4f;

    public override void Use(EnemyAI enemyAI, EnemyStatus status)
    {
        Vector3 spawnPos = enemyAI.transform.position + enemyAI.transform.forward * spawnDistance;

        Instantiate(iceFieldPrefab, spawnPos, Quaternion.identity);
    }
}
