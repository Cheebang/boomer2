public class ArmorPickup : Pickup {
    public override void PickUp() {
        FindObjectOfType<HealthManager>().PickUpArmor(gameObject, objectName);
    }
}
