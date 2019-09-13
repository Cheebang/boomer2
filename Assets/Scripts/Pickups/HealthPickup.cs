public class HealthPickup : Triggerable {
    public override void PickUp() {
        base.PickUp();
        FindObjectOfType<HealthManager>().PickUpHealth(gameObject, objectName);
    }
}
