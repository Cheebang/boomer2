using UnityEngine;

public abstract class Triggerable : MonoBehaviour {
    public string objectName;
    public string uniqueId;
    public bool interacted { get; protected set; } = false;

    private InteractiblesCollection interactibles;

    public void Start() {
        uniqueId = UniqueId.Generate(gameObject);
        interactibles = FindObjectOfType<InteractiblesCollection>();
        GameEvents.LoadInitiated += Load;
    }

    public virtual void SwitchState() { }
    public virtual void Interact() { }

    public void Load() {
        bool wasInteracted = false;
        interactibles.Interactibles.TryGetValue(uniqueId, out wasInteracted);
        interacted = wasInteracted;
    }

    private void OnDestroy() {
        GameEvents.LoadInitiated -= Load;
    }
}
