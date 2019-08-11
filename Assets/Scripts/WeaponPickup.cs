public class WeaponPickup : Triggerable {
    public override void Interact() {
        FindObjectOfType<FireWeapon>().PickUpWeapon(gameObject);
    }
}
