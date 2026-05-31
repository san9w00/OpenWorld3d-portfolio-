using UnityEngine;

public class DoorDG : MonoBehaviour
{
    [SerializeField] private float openHeight = 4f;
    [SerializeField] private float moveSpeed = 3f;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip doorSFX;

    private Vector3 closedPos;
    private Vector3 openPos;
    private bool isOpen;

    private AudioSource audioSource;
    private bool isMoving = false; // 현재 문이 움직이는지.

    private void Start()
    {
        closedPos = transform.position;
        openPos = closedPos + Vector3.up * openHeight;

        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = doorSFX;
        audioSource.loop = true;
    }

    private void Update()
    {
        Vector3 target = isOpen ? openPos : closedPos;

        // 목적지까지의 거리 체크
        float distance = Vector3.Distance(transform.position, target);

        if (distance > 0.001f) // 문이 아직 움직이는 중이라면
        {
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);

            // 움직이기 시작했는데 소리가 안 나고 있었다면 재생
            if (!isMoving)
            {
                isMoving = true;
                if (audioSource.clip != null && !audioSource.isPlaying)
                {
                    audioSource.Play();
                }
            }
        }
        else // 목적지에 도달했다면
        {
            if (isMoving)
            {
                isMoving = false;
                audioSource.Stop(); // 소리 즉시 정지
            }
        }
    }

    public void OpenDoor(bool value)
    {
        // 발판을 밟거나 뗐을 때 상태가 변하는 순간
        if (isOpen != value)
        {
            isOpen = value;

            // 열리다 갑자기 닫히거나, 닫히다 갑자기 열릴 때 소리를 처음부터 다시 깔끔하게 재생
            if (audioSource.clip != null)
            {
                audioSource.Stop();
                audioSource.Play();
                isMoving = true;
            }
        }
    }
}
