public class HealthPickup : Triggerable {
    public override void PickUp() {
        FindObjectOfType<HealthManager>().PickUpHealth(gameObject);
    }
}
