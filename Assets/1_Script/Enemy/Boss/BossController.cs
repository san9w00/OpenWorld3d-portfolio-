using UnityEngine;
using UnityEngine.AI;

public class BossController : MonoBehaviour
{
    public Transform player;
    public NavMeshAgent agent;
    public Animator animator;

    [Header("거리 설정")]
    public float detectRange = 25f;
    public float jumpAttackRange = 20f;
    public float attackRange = 6f;

    [Header("Attack HitBox")]
    [SerializeField] private Vector3 hitBoxPos;
    [SerializeField] private Vector3 hitBoxSize = new Vector3(2, 2, 2);
    [SerializeField] private LayerMask targetLayer;

    private BossStatus status;
    private BossStateMachine stateMachine;

    // 상태
    public BossIdleState idleState;
    public BossChaseState chaseState;
    public BossAttackState attackState;
    public BossJumpAttackState jumpAttackState;

    private void Awake()
    {
        status = GetComponent<BossStatus>();

        stateMachine = new BossStateMachine();

        idleState = new BossIdleState(this, stateMachine);
        chaseState = new BossChaseState(this, stateMachine);
        attackState = new BossAttackState(this, stateMachine);
        jumpAttackState = new BossJumpAttackState(this, stateMachine);
    }

    private void Start()
    {
        stateMachine.ChangeState(idleState);

        // 플레이어 자동 찾기
        if (player == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");

            if (playerObj != null)
            {
                player = playerObj.transform;
            }
            else
            {
                Debug.LogError("Player를 찾지 못함! Tag 확인해라");
            }
        }
    }

    private void Update()
    {
        stateMachine.Update();
    }

    public float DistanceToPlayer()
    {
        return Vector3.Distance(transform.position, player.position);
    }

    // Animation Event
    public void ApplyDamage()
    {
        Vector3 center = transform.position + transform.forward * hitBoxPos.z
                     + transform.right * hitBoxPos.x
                     + transform.up * hitBoxPos.y;

        Collider[] hits = Physics.OverlapBox(
        center,
        hitBoxSize * 0.5f,
        transform.rotation,
        targetLayer
        );

        foreach (var hit in hits)
        {
            IDamageable damageable = hit.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(status.attackDamage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Vector3 center = transform.position + transform.forward * hitBoxPos.z
                         + transform.right * hitBoxPos.x
                         + transform.up * hitBoxPos.y;

        Gizmos.matrix = Matrix4x4.TRS(center, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, hitBoxSize);
    }
}
