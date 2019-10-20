public class AmmoPickup : Pickup {
    public override void PickUp() {
        FindObjectOfType<WeaponManager>().PickUpAmmo(gameObject);
    }
}
