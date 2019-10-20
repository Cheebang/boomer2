public class WeaponPickup : Pickup {
    public override void PickUp() {
        FindObjectOfType<WeaponManager>().PickUpWeapon(gameObject);
    }
}
