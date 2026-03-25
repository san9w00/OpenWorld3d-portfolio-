using System;
using UnityEngine;

public enum StatType
{
    HP,
    Stamina,
    Gold,
    Level,
    Exp
}

public class PlayerStatus : MonoBehaviour, IDamageable
{
    [SerializeField] private PlayerData data;
    public float curHP { get; private set; } 
    public float curStamina { get; private set; }
    public int Gold { get; private set; } = 200;

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
    public float RunCostPerSecond => data.runStaminaCost;
    public float ShieldCost => data.shieldStaminaCost;

    public event Action<StatType, float, float> OnStatChanged;
    public Action OnGoldChanged;

    public bool IsUsingStamina { get; set; }


    private void Awake()
    {
        curHP = MaxHP;
        curStamina = MaxStamina;
    }

    private void OnEnable()
    {
        EventBus.Subscribe<GoldRewardEvent>(OnGoldReward);
    }

    private void Start()
    {
        OnStatChanged?.Invoke(StatType.HP, curHP, MaxHP);
        OnStatChanged?.Invoke(StatType.Stamina, curStamina, MaxStamina);
    }

    private void Update()
    {
        // 자동 스테미나 회복
        if (!IsUsingStamina && curStamina < MaxStamina)
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
        PlayerController controller = GetComponent<PlayerController>();

        // 방어중이면 피해 무효
        if (controller != null && controller.IsGuarding())
        {
            Debug.Log("방어 성공!");
            return;
        }

        float finalDamage = Mathf.Max(1, damage - Defense);
        curHP = Mathf.Clamp(curHP - finalDamage, 0, MaxHP);

        OnStatChanged?.Invoke(StatType.HP, curHP, MaxHP);

        Debug.Log($"플레이어 남은 체력 : {curHP}");

        if (curHP <= 0)
        {
            Die();
        }
    }

    private void OnGoldReward(GoldRewardEvent evt)
    {
        AddGold(evt.Amount);

        Debug.Log($"+{evt.Amount} 골드 획득!");
    }

    public void AddGold(int amount)
    {
        Gold += amount;
        OnGoldChanged?.Invoke();
    }

    public bool TrySpendGold(int amount)
    {
        if (Gold < amount)
            return false;

        Gold -= amount;
        OnGoldChanged?.Invoke();
        return true;
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
