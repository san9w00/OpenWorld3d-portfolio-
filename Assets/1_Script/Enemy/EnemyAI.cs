using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

    [Header("Target")]
    public Transform target;

    [Header("Range Settings")]
    public float pursueRange = 15f;
    public float attackRange = 3f;

    [Header("Attack HitBox")]
    [SerializeField] private Vector3 hitBoxPos;
    [SerializeField] private Vector3 hitBoxSize = new Vector3(2, 2, 2);
    [SerializeField] private LayerMask targetLayer;

    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Animator animator;

    private EnemyStateMachine _enemySM;
    private EnemyStatus _enemyStatus;

    protected virtual void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        _enemySM = GetComponent<EnemyStateMachine>();
        _enemyStatus = GetComponent<EnemyStatus>();
    }

    private void Update()
    {
        _enemySM.RunState();
    }

    public float DistanceToTarget()
    {
        return Vector3.Distance(transform.position, target.position);
    }

    public void Transition(State state)
    {
        _enemySM.Transition(state);
    }


    // Animation Event
    public void ApplyDamage()
    {
        float damage = _enemyStatus.Data.atkDamage;

        BossAI boss = GetComponent<BossAI>();
        if (boss != null && boss.currentAttackDamage > 0)
        {
            damage = boss.currentAttackDamage;
        }

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
                damageable.TakeDamage(_enemyStatus.Data.atkDamage);
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
