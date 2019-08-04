using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;

public class HealthManager : MonoBehaviour {
    public int health = 100;
    public int armor = 0;
    private FirstPersonController fpsController;
    private FireWeapon fireWeapon;
    private HUDController hudController;
    private bool dead;
    private float startDeadTimer = 1f;
    private float deadTimer;
    private int maxHealth = 100;
    private int maxArmor = 100;

    void Start() {
        deadTimer = startDeadTimer;
        fpsController = GetComponent<FirstPersonController>();
        fireWeapon = GetComponent<FireWeapon>();
        hudController = FindObjectOfType<HUDController>();
    }

    void Update() {
        if (dead) {
            hudController.OpenMessagePanel("You are dead\nClick to restart");
            if (deadTimer <= 0 && Input.GetButtonUp("Fire1")) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                deadTimer = startDeadTimer;
            }
            else {
                deadTimer -= Time.deltaTime;
            }
        }
    }

    public void PickUpHealth(GameObject item) {
        if (health < maxHealth) {
            hudController.Log("picked up " + item.name);
            health += 25;
            item.SetActive(false);
            if (health > maxHealth) {
                health = maxHealth;
            }
        }
        else {
            hudController.Log("health at max");
        }
    }

    public void PickUpArmor(GameObject item) {
        if (armor < maxArmor) {
            hudController.Log("picked up " + item.name);
            armor += 25;
            item.SetActive(false);
            if (armor > maxArmor) {
                armor = maxArmor;
            }
        }
        else {
            hudController.Log("armor at max");
        }
    }

    public void TakeDamage(int amount) {
        if (dead) {
            return;
        }

        int finalAmount = amount;

        if (armor > 0) {
            finalAmount = amount / 2;
            armor -= finalAmount;
            if (armor < 0) {
                finalAmount += armor;
                armor = 0;
            }
        }

        health -= finalAmount;
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
