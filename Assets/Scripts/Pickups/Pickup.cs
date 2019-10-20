using UnityEngine;

public abstract class Pickup : MonoBehaviour {
    public string objectName;
    public string uniqueId;

    private CollectibleItemSet collectibleItems;

    public void Start() {
        uniqueId = UniqueId.Generate(gameObject);
        collectibleItems = FindObjectOfType<CollectibleItemSet>();
        GameEvents.LoadInitiated += Load;
    }

    public virtual void PickUp() { }

    public void Load() {
        if (collectibleItems.CollectedItems.Contains(uniqueId)) {
            gameObject.SetActive(false);
        }
    }

    private void OnDestroy() {
        GameEvents.LoadInitiated -= Load;
    }
}
