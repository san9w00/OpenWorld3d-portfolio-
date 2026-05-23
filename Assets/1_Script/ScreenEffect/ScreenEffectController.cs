using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ScreenEffectController : MonoBehaviour
{
    [Header("Volume")]
    [SerializeField] private Volume volume;

    [Header("Hit Effect")]
    [SerializeField] private float hitDuration = 0.15f;
    [SerializeField] private float fadeSpeed = 8f;

    private Vignette vignette;
    private Coroutine hitCoroutine;

    private void Awake()
    {
        volume.profile.TryGet(out vignette);
    }

    private void Start()
    {
        vignette.intensity.Override(0f);
    }

    private void OnEnable()
    {
        EventBus.Subscribe<PlayerHitEvent>(OnPlayerHit);
    }

    private void OnDisable()
    {
        EventBus.UnSubscribe<PlayerHitEvent>(OnPlayerHit);
    }

    private void OnPlayerHit(PlayerHitEvent evt)
    {
        if (hitCoroutine != null)
        {
            StopCoroutine(hitCoroutine);
        }

        hitCoroutine = StartCoroutine(HitEffectCoroutine(evt.Intensity));
    }

    private IEnumerator HitEffectCoroutine(float targetIntensity)
    {
        vignette.color.Override(Color.red);

        // 즉시 강하게
        vignette.intensity.Override(targetIntensity);

        yield return new WaitForSeconds(hitDuration);

        // 부드럽게 감소
        while (vignette.intensity.value > 0.01f)
        {
            float current = vignette.intensity.value;

            current = Mathf.Lerp(current, 0f, Time.deltaTime * fadeSpeed);

            vignette.intensity.Override(current);

            yield return null;
        }

        vignette.intensity.Override(0f);
    }
}
