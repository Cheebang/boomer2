using UnityEngine;

public abstract class Triggerable : MonoBehaviour {
    public string objectName;
    private string uniqueId;
    private CollectibleItemSet collectibleItems;

    public void Start() {
        uniqueId = UniqueId.Generate(gameObject);
        collectibleItems = FindObjectOfType<CollectibleItemSet>();
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
    }

    private void OnDestroy() {
        GameEvents.LoadInitiated -= Load;
    }
}
