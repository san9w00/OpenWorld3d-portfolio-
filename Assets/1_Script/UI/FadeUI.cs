using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeUI : MonoBehaviour
{
    public static FadeUI Instance;

    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeSpeed = 2f;

    private void Awake()
    {
        Instance = this;
    }

    public IEnumerator FadeOut()
    {
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * fadeSpeed;
            SetAlpha(t);
            yield return null;
        }
    }

    public IEnumerator FadeIn()
    {
        float t = 1;

        while(t > 0)
        {
            t -= Time.deltaTime * fadeSpeed;
            SetAlpha(t);
            yield return null;
        }
    }

    void SetAlpha(float alpha)
    {
        Color c = fadeImage.color;
        c.a = alpha;
        fadeImage.color = c;
    }
}
