using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    [Header("Settings")]
    [SerializeField] private float fakeLoadingDuration = 4f;
    [SerializeField] private float fadeDuration = 1f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneRoutine(sceneName));
    }

    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        // UI 표시
        SceneTransitionUI.Instance.Show();

        // Fade In
        yield return StartCoroutine(SceneTransitionUI.Instance.FadeIn(fadeDuration));

        // 로드 시작
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        asyncLoad.allowSceneActivation = false;

        float timer = 0;

        // 로딩
        while (timer < fakeLoadingDuration)
        {
            timer += Time.deltaTime;

            float progress = timer / fakeLoadingDuration;

            SceneTransitionUI.Instance.SetProgress(progress);

            yield return null;
        }

        // 로딩 완료까지 대기
        while (asyncLoad.progress < 0.9f)
        {
            yield return null;
        }

        // 로딩바 꽉 채우기
        SceneTransitionUI.Instance.SetProgress(1f);

        yield return new WaitForSeconds(0.5f);

        // 씬 활성화
        asyncLoad.allowSceneActivation = true;

        // 씬 완전히 로드될 때까지 대기
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.3f);

        // Fade Out
        yield return StartCoroutine(SceneTransitionUI.Instance.FadeOut(fadeDuration));

        // UI 숨김
        SceneTransitionUI.Instance.Hide();
    }
}
