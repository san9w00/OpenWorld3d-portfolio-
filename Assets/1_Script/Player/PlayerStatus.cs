using System;
using UnityEngine;

public enum StatType
{
    HP,
    Stamina,
    Level,
    Exp
}

public class PlayerStatus : MonoBehaviour, IDamageable
{
    [SerializeField] private PlayerData data;
    public float curHP { get; private set; } 
    public float curStamina { get; private set; }

    // Base Stats
    public float MaxHP => data.maxHP + bonusMaxHP;
    public float MaxStamina => data.maxStamina + bonusMaxStamina;
    public float AtkDamage => data.atkDamage + bonusAtkDamage;
    public float Defense => data.defense + bonusDefense;  
    public float MoveSpeed => data.moveSpeed;
    public float JumpPower => data.jumpPower;   

    // Bonus
    private float bonusMaxHP;
    private float bonusMaxStamina;
    private float bonusAtkDamage;
    private float bonusDefense;

    // Stamina
    public float StaminaRegenRate => data.staminaRegenRate;
    public float RollCost => data.rollStaminaCost;
    public float AttackCost => data.attackStaminaCost;

    public event Action<StatType, float, float> OnStatChanged;


    private void Awake()
    {
        curHP = MaxHP;
        curStamina = MaxStamina;
    }

    private void Start()
    {
        OnStatChanged?.Invoke(StatType.HP, curHP, MaxHP);
        OnStatChanged?.Invoke(StatType.Stamina, curStamina, MaxStamina);
    }

    private void Update()
    {
        // 자동 스테미나 회복
        if (curStamina < MaxStamina)
        {
            curStamina += StaminaRegenRate * Time.deltaTime;
            curStamina = Mathf.Min(curStamina, MaxStamina);

            OnStatChanged?.Invoke(StatType.Stamina, curStamina, MaxStamina);
        }
    }

    // 스테미나 사용 가능 확인 및 소모
    public bool UseStamina(float amount)
    {
        if (curStamina >= amount)
        {
            curStamina -= amount;
            OnStatChanged?.Invoke(StatType.Stamina, curStamina, MaxStamina);
            return true;
        }
        Debug.Log($"스테미나 부족! 현재 양: {curStamina} / 소모 필요량: {amount}");
        return false; // 스테미나 부족
    }

    public void TakeDamage(float damage)
    {
        float finalDamage = Mathf.Max(1, damage - Defense);
        curHP = Mathf.Clamp(curHP - finalDamage, 0, MaxHP);

        OnStatChanged?.Invoke(StatType.HP, curHP, MaxHP);

        Debug.Log($"플레이어 남은 체력 : {curHP}");

        if (curHP <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        curHP += amount;
        curHP = Mathf.Clamp(curHP, 0, MaxHP);

        OnStatChanged?.Invoke(StatType.HP, curHP, MaxHP);

        Debug.Log("플레이어 회복! / 현재 체력" + curHP);
    }

    private void Die()
    {
        Debug.Log("플레이어 사망!");
    }

    // -- 업그레이드 증가 메서드 --
    public void IncreaseMaxHP(float amount)
    {
        bonusMaxHP += amount;
        curHP = MaxHP;
        OnStatChanged?.Invoke(StatType.HP, curHP, MaxHP);
    }

    public void IncreaseAttack(float amount)
    {
        bonusAtkDamage += amount;
    }

    public void IncreaseDefense(float amount)
    {
        bonusDefense += amount;
    }
}
