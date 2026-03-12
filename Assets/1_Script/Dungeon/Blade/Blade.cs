using UnityEngine;

public class Blade : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float rotateSpeed = 300f;

    private int direction = 1;

    private void Update()
    {
        transform.Translate(Vector3.right * direction * moveSpeed * Time.deltaTime, Space.World);
        transform.Rotate(Vector3.forward * direction * rotateSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Trigger"))
        {
            direction *= -1;
        }       
    }
}
