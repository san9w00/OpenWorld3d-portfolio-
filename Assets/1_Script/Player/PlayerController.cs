using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Components")]
    private PlayerStatus _playerStatus;
    private CharacterController _characterController;
    private Animator _animator;

    [Header("Movement Settings")]
    [SerializeField] private float gravityMultiplier = 3.0f; // 중력 가속도 조절 변수
    private Vector3 _velocity; // 중력, 점프력 계산할 변수

    [Header("Roll Settings")]
    [SerializeField] private float rollSpeed = 10f;
    [SerializeField] private float rollDuration = 0.5f;
    private bool _isRolling = false; // 구르는 중인지 체크

    [Header("Attack Settings")]
    private WeaponType currentWeaponType = WeaponType.Unarmed; // 현재 무기 타입 (애니메이션 전환용)
    private int _comboStep = 0; // 현재 콤보 단계
    private bool _canCombo = false;
    private bool _isAttacking = false;
    private bool _comboQueued = false;

    private void Awake()
    {
        _playerStatus = GetComponent<PlayerStatus>();
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        PlayerManager.Instance.RegisterPlayer(gameObject);
    }

    private void Update()
    {
        if (!_characterController.enabled) return;

        // 중력
        if (_characterController.isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }

        _velocity.y += Physics.gravity.y * gravityMultiplier * Time.deltaTime;
        _characterController.Move(_velocity * Time.deltaTime);
    }

    // [이동]
    public void Move(Vector3 direction)
    {
        if (_isRolling || _isAttacking) return;

        float speed = 0f;

        if (direction.magnitude >= 0.1f)
        {
            // 이동 로직
            speed = _playerStatus.MoveSpeed;
            _characterController.Move(direction * speed * Time.deltaTime);

            // 회전 로직
            Vector3 targetDirection = Vector3.Slerp(transform.forward, direction, 0.15f);
            transform.rotation = Quaternion.LookRotation(targetDirection);
        }
        if (_animator != null)
        {
            _animator.SetFloat("Speed", direction.magnitude);
        }
    }

    // 달리기
    public void Run(Vector3 direction)
    {
        if (_isRolling || _isAttacking) return;

        // 탈진 상태면 달리기 금지
        if (_playerStatus.IsExhausted)
        {
            Move(direction);
            return;
        }

        float speed = 0f;

        float staminaCost = _playerStatus.RunCostPerSecond * Time.deltaTime;
        if (!_playerStatus.UseStamina(staminaCost))
        {
            Move(direction);
            return;
        }

        if (direction.magnitude >= 0.1f)
        {
            speed = _playerStatus.MoveSpeed * 2f; // 달리기 배율

            _characterController.Move(direction * speed * Time.deltaTime);

            Vector3 targetDirection = Vector3.Slerp(transform.forward, direction, 0.15f);
            transform.rotation = Quaternion.LookRotation(targetDirection);
        }

        if (_animator != null)
        {
            _animator.SetFloat("Speed", direction.magnitude * 3);
        }
    }

    // [점프]
    public void Jump()
    {
        if (_isRolling || _isAttacking) return;

        if (_characterController.isGrounded)
        {
            float jumpPower = _playerStatus.JumpPower;
            _velocity.y = Mathf.Sqrt(jumpPower * -2f * Physics.gravity.y);

            if (_animator != null)
            {
                _animator.SetTrigger("Jump");
            }
        }
    }

    // [공격]
    public void Attack()
    {
        // 탈진 상태면 공격 금지
        if (_playerStatus.IsExhausted)
            return;

        if (_isRolling ) return;

        if (!_isAttacking)
        {
            if (_playerStatus.UseStamina(_playerStatus.AttackCost))
            {
                StartAttack(1);
            }
        }
        // 입력 저장만
        else if (_canCombo && _comboStep < 2)
        {
            TryComboAttack();
        }
    }

    public void StartAttack(int step)
    {
        _isAttacking = true;
        _comboStep = step;

        _animator.SetInteger("ComboStep", _comboStep);

        if (step == 1)
        {
            _animator.SetTrigger("Attack");
        }
    }

    private void TryComboAttack()
    {
        if (!_canCombo)
            return;

        if (!_playerStatus.UseStamina(_playerStatus.AttackCost))
            return;

        _comboQueued = false;
        _canCombo = false;

        _comboStep = 2;

        _animator.SetInteger("ComboStep", 2);
    }

    // 애니메이션 이벤트

    public void OnEnableCombo()
    {
        _canCombo = true;
    }

    public void OnAttackEnd()
    {
        _isAttacking = false;
        _canCombo = false;

        _comboStep = 0;
        _animator.SetInteger("ComboStep", 0);
    }

    // [구르기]
    public void Roll(Vector3 direction)
    {
        // 탈진 상태면 회피 금지
        if (_playerStatus.IsExhausted)
            return;

        if (_isRolling || _isAttacking || direction.magnitude < 0.1f || !_characterController.isGrounded) return;

        if (_playerStatus.UseStamina(_playerStatus.RollCost))
        {
            StartCoroutine(RollRoutine(direction));
        }
    }

    private IEnumerator RollRoutine(Vector3 direction)
    {
        _isRolling = true;

        if (_animator != null) _animator.SetTrigger("Roll");

        float startTime = Time.time;
        while (Time.time < startTime + rollDuration)
        {
            _characterController.Move(direction * rollSpeed * Time.deltaTime);
            transform.rotation = Quaternion.LookRotation(direction);
            yield return null;
        }

        _isRolling = false;
    }

    // 충돌 시 Rigidbody에 힘 가하기
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rb = hit.collider.attachedRigidbody;

        if (rb == null || rb.isKinematic)
            return;

        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
        rb.linearVelocity = pushDir * 2f;
    }

    public void SetWeaponType(WeaponType type)
    {
        currentWeaponType = type;

        if (_animator != null)
        {
            _animator.SetInteger("WeaponType", (int)type);
        }
    }



    // ----------애니메이션 이벤트 (사운드)------------

    // (발소리)
    public void PlayFootStep()
    {
        SoundManager.Instance.PlaySFX(SFXType.FootStep);
    }

    // (공격 휙소리)
    public void PlayAttackSwingSFX()
    {
        PlayerEquipment equipment = GetComponent<PlayerEquipment>();

        if (equipment == null) return;

        WeaponItemSO weapon = equipment.CurrentWeaponData;

        if (weapon == null) return;

        SoundManager.Instance.PlaySFX(weapon.swingSFX);
    }
}
