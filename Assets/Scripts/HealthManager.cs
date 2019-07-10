using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour {
    public int health = 100;

    public void PickUpHealth(string name) {
        if (health < 100) {
            health += 25;
            if (health > 100) {
                health = 100;
            }
        }
    }
}
