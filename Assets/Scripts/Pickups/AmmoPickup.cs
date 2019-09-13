public class AmmoPickup : Triggerable {
    public override void PickUp() {
        base.PickUp();
        FindObjectOfType<WeaponManager>().PickUpAmmo(gameObject);
    }
}
