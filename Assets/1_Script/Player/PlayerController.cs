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
    }

    // [이동]
    public void Move(Vector3 direction)
    {
        if (_isRolling || _isAttacking) return;

        float speed = 0f;

        if (direction.magnitude >= 0.1f)
        {
            // 이동 로직
            speed = _playerStatus.curMoveSpeed;
            _characterController.Move(direction * speed * Time.deltaTime);

            // 회전 로직
            Vector3 targetDirection = Vector3.Slerp(transform.forward, direction, 0.15f);
            transform.rotation = Quaternion.LookRotation(targetDirection);
        }
        if (_animator != null)
        {
            _animator.SetFloat("Speed", direction.magnitude * speed);
        }
    }

    // [점프]
    public void Jump()
    {
        if (_isRolling) return;

        if (_characterController.isGrounded)
        {
            float jumpPower = _playerStatus.curJumpPower;
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
        if (_isRolling) return;

        if (!_isAttacking)
        {
            StartAttack(1);
        }
        else if (_canCombo && _comboStep < 2)
        {
            _canCombo = false;
            _comboStep = 2;

            _animator.SetInteger("ComboStep", _comboStep);
            _animator.SetTrigger("Attack");
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
        if (_isRolling || direction.magnitude < 0.1f || !_characterController.isGrounded) return;

        StartCoroutine(RollRoutine(direction));
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
}
