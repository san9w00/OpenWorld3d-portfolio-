using UnityEngine;

public class VFXManager : MonoBehaviour
{
    [SerializeField] private VFXDatabase vfxDatabase;

    private void OnEnable()
    {
        EventBus.Subscribe<VFXEvent>(OnVFX);
        EventBus.Subscribe<LevelUpEvent>(OnLevelUp);
    }

    private void OnDisable()
    {
        EventBus.UnSubscribe<VFXEvent>(OnVFX);
        EventBus.UnSubscribe<LevelUpEvent>(OnLevelUp);
    }

    private void OnVFX(VFXEvent e)
    {
        if (e.VFXPrefab == null)
        {
            Debug.LogWarning("VFXPrefab 없음");
            return;
        }

        SpawnVFX(e.VFXPrefab, e.Position);
    }

    private void OnLevelUp(LevelUpEvent e)
    {
        if (vfxDatabase == null || vfxDatabase.levelUpVFX == null)
        {
            Debug.LogWarning("LevelUp VFX 없음");
            return;
        }

        SpawnVFX(vfxDatabase.levelUpVFX, e.Position + Vector3.up * 1.5f);
    }

    private void SpawnVFX(GameObject vfxPrefab, Vector3 position)
    {
        Instantiate(vfxPrefab, position, Quaternion.identity);
    }
}
