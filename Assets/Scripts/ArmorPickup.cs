public class ArmorPickup : Triggerable {
    public override void Interact() {
        FindObjectOfType<HealthManager>().PickUpArmor(gameObject);
    }
}
