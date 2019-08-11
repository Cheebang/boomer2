public class HealthPickup : Triggerable {
    public override void Interact() {
        FindObjectOfType<HealthManager>().PickUpHealth(gameObject);
    }
}
