using TMPro;
using UnityEngine;

public class NameUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    private Transform targetCamera;

    public void SetName(string name)
    {
        nameText.text = name;
    }

    private void Start()
    {
        targetCamera = Camera.main.transform;
    }

    private void LateUpdate()
    {
        transform.forward = targetCamera.forward;
    }
}
