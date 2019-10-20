[System.Serializable]
public abstract class Weapon {
    public string name;
    public float range = 9999f;
    public float fireRate = 0.25f;
    public int damage = 25;
    public bool collected = false;
    public int ammo = 10;
    public int maxAmmo = 299;
    public bool auto = false;
    public bool projectile = false;
    public float effectiveRange = 9999f;
    public int bulletForce = 3;
    public int pellets = 1;
    public float spreadDeviation = 0;

    public Weapon(string name) {
        this.name = name;
    }

    public void AddAmmo(int amount) {
        ammo += amount;
        if (ammo > maxAmmo) {
            ammo = maxAmmo;
        }
    }
}

[System.Serializable]
public class Melee : Weapon {
    public Melee() : base("melee") {
        range = 2f;
        fireRate = 0.5f;
        damage = 20;
        ammo = -1;
        maxAmmo = 1;
        effectiveRange = range;
        collected = true;
    }
}

[System.Serializable]
public class Pistol : Weapon {
    public Pistol() : base("pistol") {
        collected = true;
    }
}

[System.Serializable]
public class Shotgun : Weapon {
    public Shotgun() : base("shotgun") {
        range = 30f;
        fireRate = 1f;
        ammo = 5;
        maxAmmo = 99;
        collected = false;
        effectiveRange = 10f;
        bulletForce = 5;
        pellets = 5;
        spreadDeviation = 10;
    }
}

[System.Serializable]
public class MachineGun : Weapon {
    public MachineGun() : base("mg") {
        range = 100f;
        fireRate = 0.15f;
        damage = 25;
        ammo = 25;
        maxAmmo = 499;
        collected = true;
        auto = true;
        effectiveRange = 100f;
    }
}

[System.Serializable]
public class RPG : Weapon {
    public RPG() : base("rpg") {
        fireRate = 0.5f;
        damage = 700;
        ammo = -1;
        maxAmmo = 49;
        collected = true;
        projectile = true;
    }
}
