using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Collections;

public class HintView : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI hintText;
    [SerializeField] private TextMeshProUGUI achievementText;

    public TextMeshProUGUI HintText => hintText;
    public TextMeshProUGUI AchivementText => achievementText;
}
