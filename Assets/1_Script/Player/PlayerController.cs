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
    private int _comboStep = 0; // 현재 콤보 단계
    private bool _canCombo = false;
    private bool _isAttacking = false;

    [Header("Shield Settings")]
    private bool _isGuarding = false;

    private void Awake()
    {
        _playerStatus = GetComponent<PlayerStatus>();
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // 중력
        if(_characterController.isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }

        _velocity.y += Physics.gravity.y * gravityMultiplier *Time.deltaTime;
        _characterController.Move(_velocity * Time.deltaTime);

        // 방어중 스태미나 소모
        if (_isGuarding)
        {
            float staminaCost = _playerStatus.ShieldCost * Time.deltaTime;

            if (!_playerStatus.UseStamina(staminaCost))
            {
                StopGuard();
            }
        }
    }

    // [이동]
    public void Move(Vector3 direction)
    {
        if (_isRolling || _isAttacking || _isGuarding) return;

        float speed = 0f;
        _playerStatus.IsUsingStamina = false;


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
        if (_isRolling || _isAttacking || _isGuarding) return;

        float speed = 0f;
        _playerStatus.IsUsingStamina = true;

        float staminaCost = _playerStatus.RunCostPerSecond * Time.deltaTime;
        if(!_playerStatus.UseStamina(staminaCost))
        {
            Move(direction);
            return;
        }

        if (direction.magnitude >= 0.1f)
        {
            speed = _playerStatus.MoveSpeed * 2.3f; // 달리기 배율

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
        if (_isRolling || _isAttacking || _isGuarding) return;

        if (_characterController.isGrounded)
        {
            float jumpPower = _playerStatus.JumpPower;
            _velocity.y = Mathf.Sqrt(jumpPower * -2f * Physics.gravity.y);

            if(_animator != null)
            {
                _animator.SetTrigger("Jump");
            }
        }
    }

    // [공격]
    public void Attack()
    {
        if (_isRolling || _isGuarding) return;

        if (!_isAttacking)
        {
            if (_playerStatus.UseStamina(_playerStatus.AttackCost))
            {
                StartAttack(1);
            }        
        }
        else if (_canCombo && _comboStep < 2)
        {
            if (_playerStatus.UseStamina(_playerStatus.AttackCost))
            {
                _canCombo = false;
                _comboStep = 2;

                _animator.SetInteger("ComboStep", _comboStep);
                _animator.SetTrigger("Attack");
            }
        }
    }

    public void StartAttack(int step)
    {
        _isAttacking = true;
        _comboStep = step;

        _animator.SetInteger("ComboStep", _comboStep);
        _animator.SetTrigger("Attack");
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
        if (_isRolling || _isAttacking || _isGuarding || direction.magnitude < 0.1f || !_characterController.isGrounded) return;
        
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

    public void StartGuard()
    {
        if(_isRolling || _isAttacking) return;

        _isGuarding = true;

        _playerStatus.IsUsingStamina = true;

        // 방어 Animation
        if (_animator != null)
        {
            _animator.SetBool("Guard", true);
        }
    }

    public void StopGuard()
    {
        _isGuarding = false;
        _playerStatus.IsUsingStamina = false;

        if (_animator != null)
        {
            _animator.SetBool("Guard", false);
        }
    }

    public bool IsGuarding()
    {
        return _isGuarding;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rb = hit.collider.attachedRigidbody;

        if (rb == null || rb.isKinematic)
            return;

        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
        rb.linearVelocity = pushDir * 2f;
    }
}
