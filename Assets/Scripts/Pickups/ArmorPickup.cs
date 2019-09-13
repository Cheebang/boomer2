public class ArmorPickup : Triggerable {
    public override void PickUp() {
        base.PickUp();
        FindObjectOfType<HealthManager>().PickUpArmor(gameObject, objectName);
    }
}
