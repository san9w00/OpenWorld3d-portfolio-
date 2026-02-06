using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerStatus _playerStatus;
    private CharacterController _characterController;
    private Animator _animator;

    private void Awake()
    {
        _playerStatus = GetComponent<PlayerStatus>();
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
    }

    public void Move(Vector3 direction)
    {
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
}
