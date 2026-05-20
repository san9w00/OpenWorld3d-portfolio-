using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class MapUI : MonoBehaviour
{
    [SerializeField] private GameObject mapPanel;

    [SerializeField] private List<CampFireIcon> campFireIcons;

    private Dictionary<string, CampFireIcon> iconDict = new();

    private void Awake()
    {
        mapPanel.SetActive(false);

        foreach(var icon in campFireIcons)
        {
            icon.Init();
            iconDict.Add(icon.ID, icon);
        }

        EventBus.Subscribe<CampFireActivatedEvent>(OnCampFireActivated);
    }

    private void OnDestroy()
    {
        EventBus.UnSubscribe<CampFireActivatedEvent>(OnCampFireActivated);
    }

    void OnCampFireActivated(CampFireActivatedEvent e)
    {
        if(iconDict.TryGetValue(e.campFire.ID, out var icon))
        {
            icon.Activate(e.campFire);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleUpgradePanel();
        }
    }

    void ToggleUpgradePanel()
    {
        bool isOpen = !mapPanel.activeSelf;
        mapPanel.SetActive(isOpen);

        if (isOpen)
        {
            UIManager.Instance.OpenUI(mapPanel);
        }
        else
        {
            UIManager.Instance.CloseUI(mapPanel);
        }
    }

    public void Close()
    {
        mapPanel.SetActive(false);

        UIManager.Instance.CloseUI(mapPanel);
    }
}
