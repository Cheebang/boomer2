public class AmmoPickup : Pickup {
    public int amount = 10;

    public override void PickUp() {
        FindObjectOfType<WeaponManager>().PickUpAmmo(gameObject, amount);
    }
}
