public class WeaponPickup : Triggerable {
    public override void PickUp() {
        base.PickUp();
        FindObjectOfType<WeaponManager>().PickUpWeapon(gameObject);
    }
}
