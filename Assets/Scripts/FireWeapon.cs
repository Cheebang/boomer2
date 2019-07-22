using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FireWeapon : MonoBehaviour {
    public Weapon currentWeapon;
    public AudioClip shootSound;
    public AudioClip emptySound;
    public GameObject bulletHole;
    public GameObject blood;

    public bool shooting = false;
    public int bulletHoleMax = 25;

    private float bloodTime = 0.7f;
    private AudioSource audioSource;
    private WeaponAnimationController weaponController;
    private HUDController hudController;

    List<GameObject> bulletHoles = new List<GameObject>();
    private int currentBulletHoleIndex = 0;

    public List<Weapon> weapons = new List<Weapon>();
    private bool dead;
    public bool paused;

    void Start() {
        weapons.Add(new Pistol());
        weapons.Add(new Shotgun());
        weapons.Add(new MachineGun());
        currentWeapon = weapons[0];

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
        if (dead) {
            if (Input.GetButton("Fire1")) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            return;
        }
        if (paused) {
            return;
        }
        updateCurrentWeapon();
        fireCurrentWeapon();
    }

    private void fireCurrentWeapon() {
        bool autoWeaponShoot = Input.GetMouseButton(0) && currentWeapon.auto;
        bool nonAutoWeaponShoot = Input.GetButtonDown("Fire1") && !currentWeapon.auto;
        if ((autoWeaponShoot || nonAutoWeaponShoot) && !shooting && currentWeapon.ammo > 0) {
            shooting = true;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            StartCoroutine(FireShot());
            audioSource.PlayOneShot(shootSound, 0.5f);
            weaponController.shoot();
            currentWeapon.ammo = currentWeapon.ammo - 1;

            if (Physics.Raycast(ray, out hit, currentWeapon.range)) {
                if (hit.collider.gameObject == gameObject) {
                    return;
                }
                else if (hit.collider.tag == "Enemy") {
                    float playerDistance = Vector3.Distance(transform.position, hit.collider.transform.position);
                    int effectiveDamage = CalcEffectiveDamage(playerDistance);

                    EnemyController enemyController = hit.collider.gameObject.GetComponent<EnemyController>();
                    enemyController.TakeDamage(effectiveDamage);

                    GameObject bloodSplat = Instantiate(blood, hit.point, hit.collider.gameObject.transform.rotation);
                    StartCoroutine(SplatterBlood(bloodSplat));
                }
                else if (hit.collider.tag == "Projectile") {
                    return;
                }
                else {
                    DrawBulletHole(hit);
                }
            }
        }
        if (Input.GetButtonDown("Fire1") && currentWeapon.ammo == 0) {
            audioSource.PlayOneShot(emptySound, 0.5f);
        }
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

    IEnumerator SplatterBlood(GameObject bloodSplat) {
        yield return new WaitForSeconds(bloodTime);
        Destroy(bloodSplat);
    }
}
