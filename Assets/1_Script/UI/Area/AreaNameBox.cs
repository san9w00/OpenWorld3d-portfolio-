using System;
using UnityEngine;

public class AreaNameBox : MonoBehaviour
{
    [SerializeField] private string areaName;
    [SerializeField] private bool showOnlyOnce = true;

    private bool hasShown;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (showOnlyOnce && hasShown) return;

        hasShown = true;
    }
}
