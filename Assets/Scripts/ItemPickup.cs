public class ItemPickup : Triggerable {
    public override void Interact() {
        FindObjectOfType<ItemManager>().PickUpItem(gameObject);
    }
}
