using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class FireWeapon : MonoBehaviour {
    public Weapon currentWeapon;
    public AudioClip shootSound;
    public AudioClip emptySound;
    public GameObject bulletHole;
    public GameObject blood;

    public bool shooting = false;
    public int bulletHoleMax = 25;

    public List<Weapon> weapons = new List<Weapon>() { new Melee(), new Pistol(), new Shotgun(), new MachineGun(), new RPG() };
    public bool paused;
    public int projectileSpeed = 10;
    public GameObject projectile;

    private float bloodTime = 0.7f;
    private AudioSource audioSource;
    private WeaponAnimationController weaponController;
    private HUDController hudController;

    List<GameObject> bulletHoles = new List<GameObject>();
    private int currentBulletHoleIndex = 0;

    private bool dead;

    private List<string> ignoreTags = new List<string>() { "Player", "Projectile", "Weapon", "Item", "Ammo" };

    void Start() {
        currentWeapon = weapons[1];

        audioSource = GetComponent<AudioSource>();
        weaponController = FindObjectOfType<WeaponAnimationController>();
        hudController = FindObjectOfType<HUDController>();

        for (int i = 0; i < bulletHoleMax; i++) {
            bulletHoles.Add(Instantiate(bulletHole));
        }
    }

    public void PickUpWeapon(GameObject item) {
        foreach (Weapon weapon in weapons) {
            if (item.name == weapon.name) {
                if (!weapon.collected) {
                    weapon.collected = true;
                    weaponController.SwitchWeaponTo(currentWeapon.name, weapon.name);
                    currentWeapon = weapon;
                    hudController.Log("picked up " + item.name);
                    item.SetActive(false);
                }
                else if (weapon.ammo < weapon.maxAmmo) {
                    hudController.Log("picked up " + item.name);
                    weapon.AddAmmo(20);
                    item.SetActive(false);
                }
                else {
                    hudController.Log(weapon.name + " ammo is full");
                }
            }
        }
    }

    public void PickUpAmmo(GameObject item) {
        foreach (Weapon weapon in weapons) {
            if (item.name.Contains(weapon.name)) {
                if (weapon.ammo < weapon.maxAmmo) {
                    hudController.Log("picked up " + item.name);
                    weapon.AddAmmo(10);
                    item.SetActive(false);
                }
                else {
                    hudController.Log(weapon.name + " ammo is full");
                }
            }
        }
    }

    internal void Die() {
        dead = true;
    }

    void Update() {
        if (dead || paused) {
            return;
        }
        updateCurrentWeapon();
        fireCurrentWeapon();
    }

    private void fireCurrentWeapon() {
        bool autoWeaponShoot = Input.GetMouseButton(0) && currentWeapon.auto;
        bool nonAutoWeaponShoot = Input.GetButtonDown("Fire1") && !currentWeapon.auto;
        if ((autoWeaponShoot || nonAutoWeaponShoot) && !shooting && currentWeapon.ammo != 0) {
            shooting = true;

            StartCoroutine(FireShot());
            audioSource.PlayOneShot(shootSound, 0.5f);
            weaponController.shoot();
            currentWeapon.ammo = currentWeapon.ammo - 1;

            for (int i = 0; i < currentWeapon.pellets; i++) {
                float maxDeviation = currentWeapon.spreadDeviation * i;
                float xDeviation = Random.Range(-maxDeviation, maxDeviation);
                float yDeviation = Random.Range(-maxDeviation, maxDeviation);

                Vector3 bulletPos = new Vector3(Input.mousePosition.x + xDeviation, Input.mousePosition.y + yDeviation, Input.mousePosition.z);
                Ray ray = Camera.main.ScreenPointToRay(bulletPos);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, currentWeapon.range)) {
                    if (currentWeapon.projectile) {
                        FireProjectile(hit);
                        return;
                    }

                    //TODO use a layer to ignore instead
                    if (ignoreTags.Contains(hit.collider.tag)) {
                        return;
                    }

                    Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
                    if (rb != null) {
                        Vector3 direction = (hit.collider.transform.position - transform.position).normalized * currentWeapon.bulletForce;
                        rb.AddForceAtPosition(direction, hit.collider.transform.position, ForceMode.Impulse);
                    }

                    if (hit.collider.tag == "Enemy") {
                        float playerDistance = Vector3.Distance(transform.position, hit.collider.transform.position);
                        int effectiveDamage = CalcEffectiveDamage(playerDistance);

                        EnemyController enemyController = hit.collider.gameObject.GetComponent<EnemyController>();
                        if (enemyController != null) {
                            enemyController.TakeDamage(effectiveDamage);
                        }

                        GameObject bloodSplat = Instantiate(blood, hit.point, hit.collider.gameObject.transform.rotation);
                        Destroy(bloodSplat, bloodTime);
                    }
                    else if (hit.collider.tag == "Explodable") {
                        ExplodeScript exploder = hit.collider.gameObject.GetComponent<ExplodeScript>();
                        exploder.Explode();
                    }
                    else {
                        DrawBulletHole(hit);
                    }
                }
            }
        }
        if (Input.GetButtonDown("Fire1") && currentWeapon.ammo == 0) {
            audioSource.PlayOneShot(emptySound, 0.5f);
        }
    }

    private void FireProjectile(RaycastHit hit) {
        Vector3 spawnPos = transform.position + (transform.forward * 2f);
        spawnPos.y += 0.5f;
        GameObject newProjectile = Instantiate(projectile, spawnPos, transform.rotation);
        newProjectile.transform.LookAt(hit.point);
        newProjectile.GetComponent<Rigidbody>().velocity = newProjectile.transform.forward * projectileSpeed;
    }

    private int CalcEffectiveDamage(float playerDistance) {
        int effectiveDamage = currentWeapon.damage;

        if (playerDistance > currentWeapon.effectiveRange) {
            int difference = Convert.ToInt32(currentWeapon.range - playerDistance);
            effectiveDamage = Convert.ToInt32(currentWeapon.damage * (difference / currentWeapon.range));
        }

        return effectiveDamage;
    }

    private void updateCurrentWeapon() {
        for (int i = 0; i < weapons.Count; i++) {
            string weaponKey = (i + 1).ToString();
            if (Input.GetKeyDown(weaponKey) && weapons[i].collected) {
                weaponController.SwitchWeaponTo(currentWeapon.name, weapons[i].name);
                currentWeapon = weapons[i];
                Debug.Log("switched to " + currentWeapon.name);
            }
        }
    }

    private void DrawBulletHole(RaycastHit hit) {
        GameObject currentHole = bulletHoles[currentBulletHoleIndex];
        currentHole.transform.position = hit.point;
        currentHole.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
        currentHole.transform.parent = hit.transform;
        currentBulletHoleIndex++;
        if (currentBulletHoleIndex >= bulletHoleMax) {
            currentBulletHoleIndex = 0;
        }
    }

    IEnumerator FireShot() {
        yield return new WaitForSeconds(currentWeapon.fireRate);
        shooting = false;
        weaponController.finishShoot();
    }
}
