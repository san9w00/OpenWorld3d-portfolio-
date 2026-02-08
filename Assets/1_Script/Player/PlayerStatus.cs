using System;
using UnityEngine;

public enum StatType
{
    HP,
    Stamina,
}

public class PlayerStatus : MonoBehaviour
{
    [SerializeField] private PlayerData data;
    public float curHP { get; private set; }
    public float curStamina { get; private set; }

    // Base Stats
    public float curMoveSpeed => data.moveSpeed;
    public float curJumpPower => data.jumpPower;
    public float curAtkDamage => data.atkDamage;

    // Stamina
    public float rollCost => data.rollStaminaCost;
    public float attackCost => data.attackStaminaCost;

    public event Action<StatType, float, float> OnStatChanged;


    private void Awake()
    {
        curHP = data.maxHP;
        curStamina = data.maxStamina;
    }

    private void Start()
    {
        OnStatChanged?.Invoke(StatType.HP, curHP, data.maxHP);
        OnStatChanged?.Invoke(StatType.Stamina, curStamina, data.maxStamina);
    }

    private void Update()
    {
        // 자동 스테미나 회복
        if (curStamina < data.maxStamina)
        {
            curStamina += data.staminaRegenRate * Time.deltaTime;
            curStamina = Mathf.Min(curStamina, data.maxStamina);

            OnStatChanged?.Invoke(StatType.Stamina, curStamina, data.maxStamina);
        }
    }

    // 스테미나 사용 가능 확인 및 소모
    public bool UseStamina(float amount)
    {
        if (curStamina >= amount)
        {
            curStamina -= amount;
            OnStatChanged?.Invoke(StatType.Stamina, curStamina, data.maxStamina);
            return true;
        }
        Debug.Log($"스테미나 부족! 현재 양: {curStamina} / 소모 필요량: {amount}");
        return false; // 스테미나 부족
    }
}
