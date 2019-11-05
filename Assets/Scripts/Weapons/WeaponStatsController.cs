using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStatsController : MonoBehaviour {
    public string name;
    public float range = 9999f;
    public float fireRate = 0.25f;
    public int damage = 25;
    public int maxAmmo = 299;
    public bool auto = false;
    public bool projectile = false;
    public float effectiveRange = 9999f;
    public int bulletForce = 3;
    public int pellets = 1;
    public float spreadDeviation = 0;
    public AudioClip shootSound;
}
