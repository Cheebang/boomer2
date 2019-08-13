public class WeaponPickup : Triggerable {
    public override void PickUp() {
        FindObjectOfType<WeaponManager>().PickUpWeapon(gameObject);
    }
}
