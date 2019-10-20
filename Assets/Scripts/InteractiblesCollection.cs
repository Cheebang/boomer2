using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiblesCollection : MonoBehaviour {
    public Dictionary<string, bool> Interactibles { get; private set; } = new Dictionary<string, bool>();

    private void Awake() {
        GameEvents.SaveInitiated += Save;
        GameEvents.LoadInitiated += Load;
    }

    void Save() {
        Triggerable[] triggerables = FindObjectsOfType<Triggerable>();
        foreach (Triggerable triggerable in triggerables) {
            Interactibles[triggerable.uniqueId] = triggerable.interacted;
        }
        SaveLoad.Save(Interactibles, "Interactibles");
    }

    void Load() {
        Interactibles = SaveLoad.Load<Dictionary<string, bool>>("Interactibles");
    }
}
