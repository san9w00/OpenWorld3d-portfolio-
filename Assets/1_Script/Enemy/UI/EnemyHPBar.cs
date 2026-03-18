using UnityEngine;
using UnityEngine.UI;

public class EnemyHPBar : MonoBehaviour
{
    [SerializeField] private Image fillImage;

    private EnemyStatus enemy;

    private float visibleTime = 2f;
    private float timer;

    private void Start()
    {
        enemy = GetComponentInParent<EnemyStatus>();

        if (enemy != null)
        {
            enemy.OnHPChanged += UpdateHP;
        }

        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;

            if(timer <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }

    private void LateUpdate()
    {
        if (Camera.main != null)
        {
            transform.forward = Camera.main.transform.forward;
        }
    }

    private void UpdateHP(float current, float max)
    {
        fillImage.fillAmount = current / max;

        gameObject.SetActive(true);
        timer = visibleTime;
    }

    private void OnDestroy()
    {
        if (enemy != null)
        {
            enemy.OnHPChanged -= UpdateHP;
        }
    }
}
