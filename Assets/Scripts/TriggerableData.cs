using UnityEngine;

[System.Serializable]
public class TriggerableData {
    public bool isActive;
    public string name;

    public TriggerableData(Triggerable triggerable) {
        GameObject gameObject = triggerable.gameObject;
        isActive = gameObject.activeSelf;
        name = gameObject.name;
    }
}
