using UnityEngine;

public abstract class Weapon {
    public string name;
    public float range;
    public float fireRate;
    public int damage;
    public AudioClip shootSound;
    public bool collected;
    public int ammo;
    public bool auto;

    public Weapon(string name, float range, float fireRate, int damage, int ammo, bool collected, bool auto) {
        this.name = name;
        this.range = range;
        this.fireRate = fireRate;
        this.damage = damage;
        this.ammo = ammo;
        this.collected = collected;
        this.auto = auto;
    }
}

public class Pistol : Weapon {
    public Pistol() : base("pistol", 9999f, 0.25f, 25, 10, true, false) {
    }
}

public class Shotgun : Weapon {
    public Shotgun() : base("shotgun", 20f, 1f, 100, 5, false, false) {
    }
}

public class MachineGun : Weapon {
    public MachineGun() : base("mg", 100f, 0.15f, 25, 25, true, true) {
    }
}