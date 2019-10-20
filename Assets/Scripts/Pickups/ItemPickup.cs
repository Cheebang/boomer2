public class ItemPickup : Pickup {
    public override void PickUp() {
        FindObjectOfType<ItemManager>().PickUpItem(gameObject);
    }
}
