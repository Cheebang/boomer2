using UnityEngine;

public abstract class Triggerable : MonoBehaviour {
    public string objectName;
    public string uniqueId;
    public bool interacted { get; protected set; } = false;

    private CollectibleItemSet collectibleItems;
    private InteractiblesCollection interactibles;

    public void Start() {
        uniqueId = UniqueId.Generate(gameObject);
        collectibleItems = FindObjectOfType<CollectibleItemSet>();
        interactibles = FindObjectOfType<InteractiblesCollection>();
        GameEvents.LoadInitiated += Load;
    }

    public virtual void SwitchState() { }
    public virtual void Interact() { }
    public virtual void PickUp() {
        collectibleItems.CollectedItems.Add(uniqueId);
    }

    public void Load() {
        if (collectibleItems.CollectedItems.Contains(uniqueId)) {
            gameObject.SetActive(false);
        }

        bool wasInteracted = false;
        interactibles.Interactibles.TryGetValue(uniqueId, out wasInteracted);
        interacted = wasInteracted;
    }

    private void OnDestroy() {
        GameEvents.LoadInitiated -= Load;
    }
}
