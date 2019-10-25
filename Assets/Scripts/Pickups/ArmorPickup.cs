public class ArmorPickup : Pickup {
    public int amount = 25;

    public override void PickUp() {
        FindObjectOfType<HealthManager>().PickUpArmor(gameObject, amount);
    }
}
