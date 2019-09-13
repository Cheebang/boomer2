public class ItemPickup : Triggerable {
    public override void PickUp() {
        base.PickUp();
        FindObjectOfType<ItemManager>().PickUpItem(gameObject);
    }
}
