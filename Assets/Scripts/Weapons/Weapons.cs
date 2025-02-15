﻿[System.Serializable]
public abstract class Weapon {
    public string name;
    public bool collected = false;
    public int ammo = 10;
    public int maxAmmo = 299;

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
        ammo = -1;
        maxAmmo = 1;
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
        ammo = 5;
        maxAmmo = 99;
        collected = false;
    }
}

[System.Serializable]
public class MachineGun : Weapon {
    public MachineGun() : base("mg") {
        ammo = 25;
        maxAmmo = 499;
        collected = true;
    }
}

[System.Serializable]
public class RPG : Weapon {
    public RPG() : base("rpg") {
        ammo = 5;
        maxAmmo = 49;
        collected = true;
    }
}
