using UnityEngine;

[CreateAssetMenu(menuName = "Data/Enemy")]
public class EnemyData : ScriptableObject
{
    [Header("BaseStats")]
    public int EnemyId;
    public float maxHP;
    public float moveSpeed;
    public float atkDamage;
    public float defense;
    public float rewardExp;
}
