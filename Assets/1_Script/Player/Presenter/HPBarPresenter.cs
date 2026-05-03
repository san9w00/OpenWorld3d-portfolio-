using UnityEngine;

public class HPBarPresenter : MonoBehaviour
{
    [SerializeField] private UIBarView view;

    private PlayerStatus playerStatus;

    private void Start()
    {
        // PlayerStatus 찾기
        playerStatus = PlayerStatus.Instance;

        // 안전 체크
        if (playerStatus == null)
        {
            Debug.LogError("PlayerStatus Instance 없음!");
            return;
        }

        // 이벤트 구독
        playerStatus.OnHPChanged += UpdateView;
    }

    private void OnDestroy()
    {
        // 구독 해제
        if (playerStatus != null)
        {
            playerStatus.OnHPChanged -= UpdateView;
        }
    }

    private void UpdateView(StatData data)
    {
        view.SetFill(data.Normalized);
    }
}
