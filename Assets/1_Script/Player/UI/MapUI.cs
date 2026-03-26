using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class MapUI : MonoBehaviour
{
    [SerializeField] private GameObject mapPanel;

    [SerializeField] private List<CampFireIcon> campFireIcons;

    private Dictionary<string, CampFireIcon> iconDict = new();

    private InputHandler _inputHandler;

    private void Awake()
    {
        _inputHandler = FindAnyObjectByType<InputHandler>();

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
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleUpgradePanel();
        }
    }

    void ToggleUpgradePanel()
    {
        bool isOpen = !mapPanel.activeSelf;
        mapPanel.SetActive(isOpen);
        _inputHandler.SetInventoryState(isOpen);
    }

    public void Close()
    {
        mapPanel.SetActive(false);
        _inputHandler.SetInventoryState(false);
    }
}
