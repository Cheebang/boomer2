public class AmmoPickup : Triggerable {
    public override void PickUp() {
        FindObjectOfType<WeaponManager>().PickUpAmmo(gameObject);
    }
}
