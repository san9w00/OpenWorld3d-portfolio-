using UnityEngine;
using UnityEngine.SceneManagement;

public class GameClearUI : MonoBehaviour
{
    [Header("Boss")]
    [SerializeField] private EnemyStatus bossStatus;

    [Header("UI")]
    [SerializeField] private GameObject clearPanel;

    private void Awake()
    {
        // 시작할 때 클리어 패널은 꺼둔다.
        if (clearPanel != null)
            clearPanel.SetActive(false);
    }

    private void OnEnable()
    {
        // 보스가 죽었을 때 호출될 함수 등록
        if (bossStatus != null)
            bossStatus.OnDeath += ShowClearPanel;
    }

    private void OnDisable()
    {
        // 오브젝트가 꺼지거나 씬이 바뀔 때 이벤트 해제
        if (bossStatus != null)
            bossStatus.OnDeath -= ShowClearPanel;
    }

    private void ShowClearPanel()
    {
        if (clearPanel == null)
            return;

        clearPanel.SetActive(true);

        // 클리어 화면에서는 게임을 멈추고 싶다면 사용
        Time.timeScale = 0f;

        // 마우스 커서 보이게 처리
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void OnClickRestart()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnClickMainMenu()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene("MenuScene");
    }

    public void OnClickQuit()
    {
        Application.Quit();
    }
}
