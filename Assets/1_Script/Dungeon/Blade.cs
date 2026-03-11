using Unity.VisualScripting;
using UnityEngine;

public class Blade : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float rotationSpeed = 300f;

    private int direction = 1; // 방향

    private void Update()
    {
        // 이동
        transform.Translate(Vector3.right * direction * moveSpeed * Time.deltaTime, Space.World);

        // 회전
        transform.Rotate(Vector3.forward * direction * rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collision"))
        {
            direction *= -1;
        }

        if(other.CompareTag("Player"))
        {
            IDamageable damageable = other.GetComponent<IDamageable>();
            damageable.TakeDamage(30);
        }
    }
}
