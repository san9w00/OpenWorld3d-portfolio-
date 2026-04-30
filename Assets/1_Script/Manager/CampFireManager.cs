using System.Collections.Generic;
using UnityEngine;

public class CampFireManager : MonoBehaviour
{
    public static CampFireManager Instance;

    private Dictionary<string, CampFire> allCampfires = new();
    private HashSet<string> activatedCampfires = new();

    private void Awake()
    {
        Instance = this;
    }

    public void Register(CampFire campFire)
    {
        if(!allCampfires.ContainsKey(campFire.ID))
        {
            allCampfires.Add(campFire.ID, campFire);
        }
    }

    public void ActivateCampfire(string id)
    {
        if (activatedCampfires.Contains(id))
            return;

        activatedCampfires.Add(id);

        if (allCampfires.TryGetValue(id, out var campFire))
        {
            EventBus.Publish(new CampFireActivatedEvent(campFire));
        }
    }

    public bool IsActivated(string id)
    {
        return activatedCampfires.Contains(id);
    }

    public CampFire Get(string id)
    {
        return allCampfires.TryGetValue(id, out var cf) ? cf : null;
    }

    public List<string> GetAllActivated()
    {
        return new List<string>(activatedCampfires);
    }
}
