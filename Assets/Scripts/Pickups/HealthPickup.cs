public class HealthPickup : Pickup {
    public override void PickUp() {
        FindObjectOfType<HealthManager>().PickUpHealth(gameObject, objectName);
    }
}
