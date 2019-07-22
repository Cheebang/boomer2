using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class HealthManager : MonoBehaviour {
    public int health = 100;
    private FirstPersonController fpsController;
    private FireWeapon fireWeapon;
    private HUDController hudController;
    private bool dead;

    void Start() {
        fpsController = GetComponent<FirstPersonController>();
        fireWeapon = GetComponent<FireWeapon>();
        hudController = FindObjectOfType<HUDController>();
    }

    void Update() {
        if (dead) {
            hudController.OpenMessagePanel("You are dead\nClick to restart");
        }
    }

    public void PickUpHealth(GameObject item) {
        if (health < 100) {
            hudController.Log("picked up " + item.name);
            health += 25;
            item.SetActive(false);
            if (health > 100) {
                health = 100;
            }
        }
        else {
            hudController.Log("health at max");
        }
    }

    public void TakeDamage(int amount) {
        if (dead) {
            return;
        }

        health -= amount;
        if (health <= 0) {
            health = 0;
            Die();
        }
    }

    private void Die() {
        dead = true;
        fpsController.Die();
        fireWeapon.Die();
    }
}
