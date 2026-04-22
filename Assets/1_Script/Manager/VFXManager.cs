using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    [SerializeField] private VFXDatabase database;

    private Dictionary<(VFXActionType, VFXSwordType), Queue<GameObject>> pool = new();

    private void OnEnable()
    {
        EventBus.Subscribe<VFXEvent>(OnVFX);
    }

    private void OnDisable()
    {
        EventBus.UnSubscribe<VFXEvent>(OnVFX);
    }

    private void Awake()
    {
        InitPool();
    }

    private void InitPool()
    {
        foreach (var entry in database.vfxList)
        {
            var key = (entry.actionType, entry.swordType);

            if (!pool.ContainsKey(key))
                pool[key] = new Queue<GameObject>();

            for (int i = 0; i < entry.poolSize; i++)
            {
                GameObject obj = Instantiate(entry.prefab);
                obj.SetActive(false);
                pool[key].Enqueue(obj);
            }
        }
    }

    private void OnVFX(VFXEvent e)
    {
        GameObject prefab = database.GetPrefab(e.ActionType, e.SwordType);
        if (prefab == null) return;

        var key = (e.ActionType, e.SwordType);

        GameObject obj = GetFromPool(key, prefab);
        obj.transform.position = e.Position;
        obj.SetActive(true);

        StartCoroutine(ReturnToPoolAfterTime(key, obj, 5f));
    }

    private GameObject GetFromPool((VFXActionType, VFXSwordType) key, GameObject prefab)
    {
        if (!pool.ContainsKey(key))
            pool[key] = new Queue<GameObject>();

        if (pool[key].Count > 0)
            return pool[key].Dequeue();

        return Instantiate(prefab);
    }

    private System.Collections.IEnumerator ReturnToPoolAfterTime((VFXActionType, VFXSwordType) key, GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);

        obj.SetActive(false);
        pool[key].Enqueue(obj);
    }
}
