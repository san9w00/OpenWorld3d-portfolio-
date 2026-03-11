using UnityEngine;

public class MapUI : MonoBehaviour
{
    [SerializeField] private GameObject mapPanel;

    private InputHandler _inputHandler;

    private void Awake()
    {
        _inputHandler = FindAnyObjectByType<InputHandler>();

        mapPanel.SetActive(false);
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
