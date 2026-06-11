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

    // Hint 상태
    private bool staminaWarningShown;
    private bool lowHealthWarningShown;
    private bool exhaustedHintShown;

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
        EventBus.Subscribe<LevelUpEvent>(OnLevelUp);
    }

    private void OnDisable()
    {
        EventBus.UnSubscribe<GoldRewardEvent>(OnGoldReward);
        EventBus.UnSubscribe<LevelUpEvent>(OnLevelUp);
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

            exhaustedHintShown = false;

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
            curStamina = 0; // 완전 소진 처리
            lastStaminaUseTime = Time.time;

            OnStaminaChanged?.Invoke(new StatData(curStamina, MaxStamina)); // UI 갱신
            OnStaminaVisibleChanged?.Invoke(true); // UI 표시

            EnterExhaustedState(); // 탈진 상태 진입

            Debug.Log("스태미나 완전 소진!");
            return false;
        }

        curStamina -= amount;

        CheckStaminaHint();
        lastStaminaUseTime = Time.time;

        OnStaminaChanged?.Invoke(new StatData(curStamina, MaxStamina));
        OnStaminaVisibleChanged?.Invoke(true);

        if (curStamina <= 0.01f)
        {
            curStamina = 0;
            EnterExhaustedState(); // 탈진
        }

        return true;
    }

    // 탈진 상태 진입
    private void EnterExhaustedState()
    {
        IsExhausted = true;

        if (!exhaustedHintShown)
        {
            exhaustedHintShown = true;

            EventBus.Publish(new HintEvent("스테미나를 사용할수 없다!", HintType.Danger));
        }

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

        CheckHealthHint();

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

        // 플레이어의 체력 검사 함수
        CheckHealthHint();

        Debug.Log("플레이어 회복! / 현재 체력" + curHP);
    }

    private void Die()
    {
        Debug.Log("플레이어 사망!");

        // 플레이어 조작 비활.
        GetComponent<PlayerController>().enabled = false;

        // 플레이어 사망 이벤트 전달
        EventBus.Publish(new PlayerDeadEvent());
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

    private void OnLevelUp(LevelUpEvent evt)
    {
        FullHeal();
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

    public void FullHeal() // 최대체력 회복
    {
        curHP = MaxHP;

        // UI 갱신
        OnHPChanged?.Invoke(new StatData(curHP, MaxHP));

        CheckHealthHint();

        Debug.Log("체력 전체 회복!");
    }

    // -- 플레이어 상태 힌트 체크 --
    private void CheckHealthHint()
    {
        float hpPercent = curHP / MaxHP;

        if (hpPercent <= 0.25f && !lowHealthWarningShown)
        {
            lowHealthWarningShown = true;

            EventBus.Publish(new HintEvent("체력이 매우 낮다!", HintType.Danger));
        }

        if (hpPercent > 0.25f)
        {
            lowHealthWarningShown = false;
        }
    }

    private void CheckStaminaHint()
    {
        float staminaPercent = curStamina / MaxStamina;

        if (staminaPercent <= 0.3f && !staminaWarningShown)
        {
            staminaWarningShown = true;

            EventBus.Publish(new HintEvent("스태미나가 곧 바닥난다!", HintType.Warning));
        }
        
        if (staminaPercent > 0.3f)
        {
            staminaWarningShown = false;
        }
    }


    // -- 업그레이드 증가 메서드 --
    public void IncreaseMaxHP(float amount)
    {
        bonusMaxHP += amount;
        curHP += amount;
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
