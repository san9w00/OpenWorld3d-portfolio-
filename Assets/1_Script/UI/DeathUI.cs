using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathUI : MonoBehaviour
{
    [Header("Root")]
    [SerializeField] private GameObject root;

    [Header("Canvas Group")]
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Ui")]
    [SerializeField] private Image background;
    [SerializeField] private TMP_Text dieText;
    [SerializeField] private Button menuBTN;

    [Header("Fade")]
    [SerializeField] private float fadeDuration = 2f;

    private bool isShowing;

    private void Awake()
    {
        root.SetActive(false);

        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        menuBTN.onClick.AddListener(OnClickMenu);
    }

    private void OnEnable()
    {
        EventBus.Subscribe<PlayerDeadEvent>(OnPlayerDead);
    }

    private void OnDisable()
    {
        EventBus.UnSubscribe<PlayerDeadEvent>(OnPlayerDead);
    }

    private void OnPlayerDead(PlayerDeadEvent evt)
    {
        Show();
    }

    private void Show()
    {
        if (isShowing)
            return;

        isShowing = true;

        root.SetActive(true);

        // UIManager¿¡ µî·Ï
        UIManager.Instance.OpenUI(root);

        StartCoroutine(FadeRoutine());
    }
    
    private IEnumerator FadeRoutine()
    {
        float time = 0f;

        Color bgColor = background.color;
        Color textColor = dieText.color;

        bgColor.a = 0f;
        textColor.a = 0f;

        background.color = bgColor;
        dieText.color = textColor;

        canvasGroup.alpha = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;

            float t = time / fadeDuration;

            bgColor.a = Mathf.Lerp(0f, 1f, t);
            textColor.a = Mathf.Lerp(0f, 1f, t);

            background.color = bgColor;
            dieText.color = textColor;

            canvasGroup.alpha = Mathf.Lerp(0f, 1f, t);

            yield return null;
        }

        bgColor.a = 1f;
        textColor.a = 1f;

        background.color = bgColor;
        dieText.color = textColor;

        canvasGroup.alpha = 1f;

        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void OnClickMenu()
    {
        Keeper.Instance.DestroyALL();

        SceneLoader.Instance.LoadScene("MenuScene");
    }
}
