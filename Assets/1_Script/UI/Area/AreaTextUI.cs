using TMPro;
using UnityEngine;

public class AreaTextUI : MonoBehaviour
{
    public static AreaTextUI Instance;

    [SerializeField] private CanvasGroup panel;
    [SerializeField] private TMP_Text titleText;
}
