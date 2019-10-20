using System.Collections.Generic;
using UnityEngine;

public class CollectibleItemSet : MonoBehaviour {
    public HashSet<string> CollectedItems { get; private set; }

    private void Awake() {
        GameEvents.SaveInitiated += Save;
        GameEvents.LoadInitiated += Load;
    }

    void Save() {
        CollectedItems = new HashSet<string>();
        Pickup[] pickups = Resources.FindObjectsOfTypeAll(typeof(Pickup)) as Pickup[];
        foreach (Pickup pickup in pickups) {
            if (!pickup.isActiveAndEnabled) {
                CollectedItems.Add(pickup.uniqueId);
            }
        }
        SaveLoad.Save(CollectedItems, "CollectedItems");
    }

    void Load() {
        CollectedItems = SaveLoad.Load<HashSet<string>>("CollectedItems");
    }
}
