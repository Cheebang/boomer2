using UnityEngine;

public abstract class Weapon {
    public string name;
    public float range;
    public float fireRate;
    public int damage;
    public AudioClip shootSound;
    public bool collected;
    public int ammo;
    public int maxAmmo;
    public bool auto;
    public float effectiveRange;

    public Weapon(string name, float range, float fireRate, int damage, int ammo, int maxAmmo, bool collected, bool auto, float effectiveRange) {
        this.name = name;
        this.range = range;
        this.fireRate = fireRate;
        this.damage = damage;
        this.ammo = ammo;
        this.maxAmmo = maxAmmo;
        this.collected = collected;
        this.auto = auto;
        this.effectiveRange = effectiveRange;
    }

    public void AddAmmo(int amount) {
        ammo += amount;
        if (ammo > maxAmmo) {
            ammo = maxAmmo;
        }
    }
}

public class Pistol : Weapon {
    public Pistol() : base("pistol", 9999f, 0.25f, 25, 10, 299, true, false, 9999f) {
    }
}

public class Shotgun : Weapon {
    public Shotgun() : base("shotgun", 20f, 1f, 100, 5, 99, false, false, 5f) {
    }
}

public class MachineGun : Weapon {
    public MachineGun() : base("mg", 100f, 0.15f, 25, 25, 499, true, true, 100f) {
    }
}