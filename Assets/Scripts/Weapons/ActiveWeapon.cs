﻿using UnityEngine;

public class ActiveWeapon : MonoBehaviour {
    public GameObject[] weapons;
    public WeaponStatsController stats;
    private int activeWeaponIndex = 1;

    private void Start() {
        updateActiveWeapon(activeWeaponIndex);
    }

    public void updateActiveWeapon(int index) {
        foreach (GameObject weapon in weapons) {
            weapon.SetActive(false);
        }
        activeWeaponIndex = index;
        weapons[index].SetActive(true);
        stats = FindObjectOfType<WeaponStatsController>();
    }
}
