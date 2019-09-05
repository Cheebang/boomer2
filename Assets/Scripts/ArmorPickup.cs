public class ArmorPickup : Triggerable {
    public override void PickUp() {
        FindObjectOfType<HealthManager>().PickUpArmor(gameObject, objectName);
    }
}
