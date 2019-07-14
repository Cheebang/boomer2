using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class HealthManager : MonoBehaviour {
    public int health = 100;
    private FirstPersonController fpsController;
    private HUDController hud;
    private bool dead;

    void Start() {
        fpsController = GetComponent<FirstPersonController>();
        hud = FindObjectOfType<HUDController>();
    }

    void Update() {
        if (dead) {
            hud.OpenMessagePanel("You are dead");
        }
    }

    public void PickUpHealth(string name) {
        if (health < 100) {
            health += 25;
            if (health > 100) {
                health = 100;
            }
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
    }
}
