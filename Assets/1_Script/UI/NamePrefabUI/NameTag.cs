using UnityEngine;

public class NameTag : MonoBehaviour
{
    [Header("Name Settings")]
    [SerializeField] private string objName;

    [Header("UI")]
    [SerializeField] private GameObject nameUIPrefab;
    [SerializeField] private Transform nameUIOffset; // 머리 위치

    [Header("Distance")]
    [SerializeField] private float showDistance = 10f; // 이름표가 보이는 최대 거리

    private Transform player;
    private GameObject nameUIInstance;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // UI 생성
        nameUIInstance = Instantiate(nameUIPrefab, nameUIOffset.position, Quaternion.identity);

        var ui = nameUIInstance.GetComponent<NameUI>();
        ui.SetName(objName);

        nameUIInstance.SetActive(false); // 처음에는 비활성화
    }

    private void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);
        bool shouldShow = distance <= showDistance;

        if (nameUIInstance.activeSelf != shouldShow)
        {
            nameUIInstance.SetActive(shouldShow);
        }
    }
}
