using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    // 현재 열려있는 UI 목록들
    private HashSet<GameObject> openUIList = new HashSet<GameObject>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // UI 열림 등록
    public void OpenUI(GameObject ui)
    {
        if (openUIList.Contains(ui))
            return;

        openUIList.Add(ui);

        UpdateCursorState();
    }

    public void CloseUI(GameObject ui)
    {
        if (!openUIList.Contains(ui))
            return;

        openUIList.Remove(ui);

        UpdateCursorState();
    }

    private void UpdateCursorState()
    {
        bool hasOpenUI = openUIList.Count > 0;

        Cursor.visible = hasOpenUI;
        Cursor.lockState = hasOpenUI ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public bool IsAnyUIOpen()
    {
        return openUIList.Count > 0;
    }
}
