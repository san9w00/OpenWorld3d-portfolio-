using UnityEngine;


public enum ResourceType
{
    Tree,
    Rock,
    None,
    Coal,
}

public class ResourceObject : MonoBehaviour, IDamageable
{
    [Header("Resource")]
    [SerializeField] private ResourceType resourceType;

    [Header("HP")]
    [SerializeField] private float maxHp = 100f;

    [Header("Drop")]
    [SerializeField] private GameObject dropPrefab;
    [SerializeField] private Transform dropPoint;

    private float currentHp;
    private int lastDropStep = 0;

    private void Awake()
    {
        currentHp = maxHp;
    }

    public void TakeDamage(float damage)
    {
        Debug.Log("자원 데미지 입음!");
        currentHp -= damage;

        int currentStep = Mathf.FloorToInt((maxHp - currentHp) / 20f);

        if (currentStep > lastDropStep)
        {
            DropResource();
            lastDropStep = currentStep;
        }

        if (currentHp <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void DropResource()
    {
        GameObject obj = Instantiate(dropPrefab, dropPoint.position, Quaternion.identity);

        Rigidbody rb = obj.GetComponent<Rigidbody>();

        if (rb != null)
        {
            Vector3 force = Vector3.up + Random.insideUnitSphere;

            rb.AddForce(force * 3f, ForceMode.Impulse);
        }

        // GamePlay Event 발행
        EventBus.Publish(new GamePlayEvent(GetGamePlayEventType()));
    }

    private GamePlayEventType GetGamePlayEventType()
    {
        switch (resourceType)
        {
            case ResourceType.Tree:
                return GamePlayEventType.BranchCollected;

            case ResourceType.Rock:
                return GamePlayEventType.RockCollected;
        }

        return GamePlayEventType.BranchCollected;
    }

    public ResourceType GetResourceType()
    {
        return resourceType;
    }
}
