using UnityEngine;


[CreateAssetMenu(menuName = "Data/PlayerStats")]
public class PlayerData : ScriptableObject
{
    [Header("Base Stats")]
    public float maxHP;
    public float maxStamina;
    public float moveSpeed;
    public float atkDamage;
    public float jumpPower;

    [Header("Stamina Costs")]
    public float attackStaminaCost = 10f; // 공격 소모량
    public float rollStaminaCost = 20f;   // 구르기 소모량
    public float staminaRegenRate = 5f;   // 초당 스테미나 회복량
}
