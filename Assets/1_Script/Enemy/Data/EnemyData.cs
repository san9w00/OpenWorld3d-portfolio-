using UnityEngine;

[CreateAssetMenu(menuName = "Data/Enemy")]
public class EnemyData : ScriptableObject
{
    [Header("BaseStats")]
    public int EnemyId;
    public EnemyType enemyType;
    public float maxHP;
    public float atkDamage;
    public float atkCooldown;
    public float defense;
    public float rewardExp;
    public int rewardGold;

    [Header("Audio")]
    public AudioClip attackSFX;

    [Header("Skill")]
    [Range(0, 100)]
    public float skillChance = 20f;

    public EnemySkillSO[] skills;
}
