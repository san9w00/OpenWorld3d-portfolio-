using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Collections;

public class HintView : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI hintText;

    public TextMeshProUGUI HintText => hintText;
}
