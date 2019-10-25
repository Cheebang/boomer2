public class HealthPickup : Pickup {
    public int amount = 25;

    public override void PickUp() {
        FindObjectOfType<HealthManager>().PickUpHealth(gameObject, amount);
    }
}
