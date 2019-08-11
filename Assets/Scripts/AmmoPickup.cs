public class AmmoPickup : Triggerable {
    public override void Interact() {
        FindObjectOfType<FireWeapon>().PickUpAmmo(gameObject);
    }
}
