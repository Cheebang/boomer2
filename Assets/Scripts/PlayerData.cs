using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class PlayerData {
    public int health;
    public int armor;
    public float[] position;
    public float[] rotation;
    public string[] items;
    public int currentLevel;
    public int[] weaponAmmo;
    public bool[] weaponCollected;
    public int currentWeapon;

    public PlayerData(GameObject player) {
        HealthManager healthManager = player.GetComponent<HealthManager>();
        health = healthManager.health;
        armor = healthManager.armor;

        WeaponManager weaponManager = player.GetComponent<WeaponManager>();
        currentWeapon = weaponManager.weapons.IndexOf(weaponManager.currentWeapon);
        List<Weapon> weapons = weaponManager.weapons;
        weaponAmmo = new int[weapons.Count];
        weaponCollected = new bool[weapons.Count];

        for (int i = 0; i < weapons.Count; i++) {
            weaponAmmo[i] = weapons[i].ammo;
            weaponCollected[i] = weapons[i].collected;
        }

        position = new float[3];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;

        rotation = new float[3];
        rotation[0] = player.transform.rotation.x;
        rotation[1] = player.transform.rotation.y;
        rotation[2] = player.transform.rotation.z;

        currentLevel = SceneManager.GetActiveScene().buildIndex;

        ItemManager itemManager = player.GetComponent<ItemManager>();
        items = itemManager.items.ToArray();
    }
}
