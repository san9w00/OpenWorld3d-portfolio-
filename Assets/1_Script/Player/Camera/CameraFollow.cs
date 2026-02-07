using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // 플레이어 연결

    void LateUpdate()
    {
        transform.position = player.position;
    }
}
