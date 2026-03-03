using UnityEngine;

[CreateAssetMenu(menuName = "Data/Enemy")]
public class EnemyData : ScriptableObject
{
    [Header("BaseStats")]
    public int EnemyId;
    public float maxHP;
    public float atkDamage;
    public float atkCooldown;
    public float defense;
    public float rewardExp;
    public float rewardGold;
}
