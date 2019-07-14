using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour {
    public float speed = 5f;
    public int damageAmount = 10;

    private HealthManager playerHealthManager;

    void Start() {
        playerHealthManager = FindObjectOfType<HealthManager>();
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            playerHealthManager.TakeDamage(damageAmount);
        }
        else if (other.gameObject.CompareTag("Enemy")) {
            return;
        }

        Destroy(gameObject);
    }
}