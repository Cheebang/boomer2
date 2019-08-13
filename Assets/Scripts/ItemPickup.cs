public class ItemPickup : Triggerable {
    public override void PickUp() {
        FindObjectOfType<ItemManager>().PickUpItem(gameObject);
    }
}
