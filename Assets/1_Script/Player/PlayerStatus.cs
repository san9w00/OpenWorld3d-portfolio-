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
    public static PlayerStatus Instance;

    [SerializeField] private PlayerData data;

    [Header("Stamina Settings")]
    [SerializeField] private float staminaRegenDelay = 0.5f;

    // 현재 스탯
    public float curHP { get; private set; } 
    public float curStamina { get; private set; }
    public int Gold { get; private set; } = 200;

    // 스테미나 상태
    public bool IsExhausted { get; private set; }

    // 마지막 스테미나 사용 시간
    private float lastStaminaUseTime;

    // 데미지 배율
    private float damageMultiplier = 1f;

    // 기본 스탯
    public float MaxHP => data.maxHP + bonusMaxHP;
    public float MaxStamina => data.maxStamina + bonusMaxStamina;
    public float AtkDamage => data.atkDamage + bonusAtkDamage;
    public float Defense => data.defense + bonusDefense;  
    public float MoveSpeed => data.moveSpeed;
    public float JumpPower => data.jumpPower;   

    // 보너스 스탯
    private float bonusMaxHP;
    private float bonusMaxStamina;
    private float bonusAtkDamage;
    private float bonusDefense;

    // Stamina
    public float StaminaRegenRate => data.staminaRegenRate;
    public float RollCost => data.rollStaminaCost;
    public float AttackCost => data.attackStaminaCost;
    public float RunCostPerSecond => data.runStaminaCost;


    // =========================
    // MVP 이벤트
    // =========================

    // HP 변경 이벤트
    public event Action<StatData> OnHPChanged;

    // 스태미나 변경 이벤트
    public event Action<StatData> OnStaminaChanged;

    // 스태미나 UI 표시 여부
    public event Action<bool> OnStaminaVisibleChanged;

    // 탈진 상태 변경
    public event Action<bool> OnExhaustedChanged;

    public Action OnGoldChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        curHP = MaxHP;
        curStamina = MaxStamina;
    }

    private void OnEnable()
    {
        EventBus.Subscribe<GoldRewardEvent>(OnGoldReward);
    }

    private void Start()
    {
        // 초기 ui 동기화
        OnHPChanged?.Invoke(new StatData(curHP, MaxHP));
        OnStaminaChanged?.Invoke(new StatData(curStamina, MaxStamina));
    }

    private void Update()
    {
        HandleStaminaRegen();
    }

    // 스테미나 회복 처리
    private void HandleStaminaRegen()
    {
        if (curStamina >= MaxStamina - 0.01f)
        {
            curStamina = MaxStamina;
            return;
        }

        // 사용후 딜레이 시간 체크
        if (Time.time < lastStaminaUseTime + staminaRegenDelay)
            return;

        // 스테미나 회복
        curStamina += StaminaRegenRate * Time.deltaTime;
        curStamina = Mathf.Min(curStamina, MaxStamina);

        // UI 갱신
        OnStaminaChanged?.Invoke(new StatData(curStamina, MaxStamina));

        // 회복중에도 UI 표시
        OnStaminaVisibleChanged?.Invoke(true);

        // 탈진 상태 해제
        if (IsExhausted && curStamina >= MaxStamina - 0.01f)
        {
            curStamina = MaxStamina;

            IsExhausted = false;

            // Presenter에게 탈진 해제 알림
            OnExhaustedChanged?.Invoke(false);
        }
    }

    // 스테미나 사용
    public bool UseStamina(float amount)
    {
        // 탈진 상태면 사용 불가
        if (IsExhausted)
        {
            Debug.Log("탈진 상태!");

            return false;
        }

        // 스테미나 부족
        if (curStamina < amount)
        {
            // 완전 소진 처리
            curStamina = 0;

            // 마지막 사용 시간 갱신
            lastStaminaUseTime = Time.time;

            // UI 갱신
            OnStaminaChanged?.Invoke(
            new StatData(curStamina, MaxStamina));

            // UI 표시
            OnStaminaVisibleChanged?.Invoke(true);

            // 탈진 상태 진입
            EnterExhaustedState();

            Debug.Log("스태미나 완전 소진!");

            return false;
        }

        // 정상 사용
        curStamina -= amount;

        // 마지막 사용 시간 저장
        lastStaminaUseTime = Time.time;

        // UI 이벤트
        OnStaminaChanged?.Invoke(new StatData(curStamina, MaxStamina));

        // UI 표시 (스테미나 사용)
        OnStaminaVisibleChanged?.Invoke(true);

        // 정확히 0 도달 시 탈진
        if (curStamina <= 0.01f)
        {
            curStamina = 0;

            EnterExhaustedState();
        }

        return true;
    }

    // 탈진 상태 진입
    private void EnterExhaustedState()
    {
        IsExhausted = true;

        // UI 색상 변경
        OnExhaustedChanged?.Invoke(true);

        // 강제 표시
        OnStaminaVisibleChanged?.Invoke(true);
    }

    public void TakeDamage(float damage)
    {
        float finalDamage = Mathf.Max(1, (damage - Defense) * damageMultiplier);
        curHP = Mathf.Clamp(curHP - finalDamage, 0, MaxHP);

        // UI 이벤트
        OnHPChanged?.Invoke(new StatData(curHP, MaxHP));

        // 피격 이벤트 발행 (Vignette 강도 전달)
        EventBus.Publish(new PlayerHitEvent(0.15f));

        EventBus.Publish(new VFXEvent(
            transform.position + Vector3.up * 1,
            VFXActionType.PlayerHit,
            VFXSwordType.None
        ));

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

        // UI 이벤트
        OnHPChanged?.Invoke(new StatData(curHP, MaxHP));

        Debug.Log("플레이어 회복! / 현재 체력" + curHP);
    }

    private void Die()
    {
        Debug.Log("플레이어 사망!");
    }

    public void ApplyDamageMultiplier(float multiplier, float duration, MonoBehaviour runner)
    {
        runner.StopAllCoroutines();
        runner.StartCoroutine(DamageMultiplierCorutine(multiplier, duration));
    }

    private System.Collections.IEnumerator DamageMultiplierCorutine(float multiplier, float duration)
    {
        damageMultiplier = multiplier;

        Debug.Log("피해 감소 시작!");

        yield return new WaitForSeconds(duration);

        damageMultiplier = 1f;

        Debug.Log("피해 감소 종료!");
    }

    // =========================
    // 골드
    // =========================
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


    // -- 업그레이드 증가 메서드 --
    public void IncreaseMaxHP(float amount)
    {
        bonusMaxHP += amount;
        curHP = MaxHP;
        OnHPChanged?.Invoke(new StatData(curHP, MaxHP));
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
