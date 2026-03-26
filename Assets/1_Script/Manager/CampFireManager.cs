using System.Collections.Generic;
using UnityEngine;

public class CampFireManager : MonoBehaviour
{
    public static CampFireManager Instance;

    private Dictionary<string, CampFire> campFireList = new();

    private void Awake()
    {
        Instance = this;
    }

    public void Register(CampFire campFire)
    {
        if(!campFireList.ContainsKey(campFire.ID))
        {
            campFireList.Add(campFire.ID, campFire);

            EventBus.Publish(new CampFireActivatedEvent(campFire));
        }
    }

    public CampFire Get(string id)
    {
        return campFireList.TryGetValue(id, out var cf) ? cf : null;
    }

    public List<CampFire> GetAllActivated()
    {
        return new List<CampFire>(campFireList.Values);
    }
}
