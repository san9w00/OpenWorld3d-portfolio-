using UnityEngine;


[CreateAssetMenu(menuName = "Data/PlayerStats")]
public class PlayerData : ScriptableObject
{
    [Header("Base Stats")]
    public float maxHP;      // 최대 체력
    public float maxStamina; // 최대 스테미나
    public float gold;       // 골드
    public float moveSpeed;  // 이동속도
    public float atkDamage;  // 공격력
    public float jumpPower;  // 점프력
    public float defense;    // 방어력

    [Header("Stamina Costs")]
    public float attackStaminaCost = 10f; // 공격 소모량
    public float rollStaminaCost = 20f;   // 구르기 소모량
    public float runStaminaCost = 10f;    // 달리기 소모량
    public float staminaRegenRate = 5f;   // 초당 스테미나 회복량
}
