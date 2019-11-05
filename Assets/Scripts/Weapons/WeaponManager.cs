using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using Random = UnityEngine.Random;

public class WeaponManager : MonoBehaviour {
    public Weapon currentWeaponState;
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
    private ActiveWeapon activeWeapon;
    private HUDController hudController;

    List<GameObject> bulletHoles = new List<GameObject>();
    private int currentBulletHoleIndex = 0;

    private bool dead;

    public LayerMask layerMask;
    private FirstPersonController fpsController;

    void Start() {
        GameEvents.SaveInitiated += Save;
        GameEvents.LoadInitiated += Load;

        currentWeaponState = weapons[1];

        audioSource = GetComponent<AudioSource>();
        fpsController = GetComponent<FirstPersonController>();
        weaponController = FindObjectOfType<WeaponAnimationController>();
        activeWeapon = FindObjectOfType<ActiveWeapon>();
        hudController = FindObjectOfType<HUDController>();

        for (int i = 0; i < bulletHoleMax; i++) {
            bulletHoles.Add(Instantiate(bulletHole));
        }
    }

    private void Save() {
        SaveLoad.Save(weapons, "weaponInventory");
    }

    private void Load() {
        if (SaveLoad.SaveExistsAt("weaponInventory")) {
            List<Weapon> loadedWeapons = SaveLoad.Load<List<Weapon>>("weaponInventory");
            weapons = loadedWeapons;
        }
    }

    public void PickUpWeapon(GameObject item) {
        foreach (Weapon weapon in weapons) {
            if (item.name == weapon.name) {
                if (!weapon.collected) {
                    weapon.collected = true;
                    activeWeapon.updateActiveWeapon(weapons.IndexOf(weapon));
                    weaponController = FindObjectOfType<WeaponAnimationController>();
                    currentWeaponState = weapon;
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

    public void PickUpAmmo(GameObject item, int amount) {
        foreach (Weapon weapon in weapons) {
            if (item.name.Contains(weapon.name)) {
                if (weapon.ammo < weapon.maxAmmo) {
                    hudController.Log("picked up " + item.name + " x" + amount);
                    weapon.AddAmmo(amount);
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
        if (!shooting && fpsController.bobbing && !fpsController.m_Jumping) {
            weaponController.startWalking();
        }
        else {
            weaponController.finishWalking();
        }
    }

    private void fireCurrentWeapon() {
        bool autoWeaponShoot = Input.GetMouseButton(0) && activeWeapon.stats.auto;
        bool nonAutoWeaponShoot = Input.GetButtonDown("Fire1") && !activeWeapon.stats.auto;
        if ((autoWeaponShoot || nonAutoWeaponShoot) && !shooting && currentWeaponState.ammo != 0) {
            shooting = true;

            StartCoroutine(FireShot());
            if (activeWeapon.stats.shootSound != null) {
                audioSource.PlayOneShot(activeWeapon.stats.shootSound, 0.5f);
            }
            weaponController.shoot();
            currentWeaponState.ammo = currentWeaponState.ammo - 1;

            for (int i = 0; i < activeWeapon.stats.pellets; i++) {
                float maxDeviation = activeWeapon.stats.spreadDeviation * i;
                float xDeviation = Random.Range(-maxDeviation, maxDeviation);
                float yDeviation = Random.Range(-maxDeviation, maxDeviation);

                Vector3 bulletPos = new Vector3(Input.mousePosition.x + xDeviation, Input.mousePosition.y + yDeviation, Input.mousePosition.z);
                Ray ray = Camera.main.ScreenPointToRay(bulletPos);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, activeWeapon.stats.range, layerMask)) {
                    if (activeWeapon.stats.projectile) {
                        FireProjectile(hit);
                        return;
                    }

                    handleRigidbodyCollision(hit);
                    EnemyController enemyController = handleEnemyCollision(ref hit);
                    ExplodeScript exploder = handleExploderCollision(ref hit);

                    if (exploder == null && enemyController == null) {
                        DrawBulletHole(hit);
                    }
                }
            }
        }
        if (Input.GetButtonDown("Fire1") && currentWeaponState.ammo == 0) {
            audioSource.PlayOneShot(emptySound, 0.5f);
        }
    }

    private static ExplodeScript handleExploderCollision(ref RaycastHit hit) {
        ExplodeScript exploder = hit.collider.gameObject.GetComponent<ExplodeScript>();
        if (exploder != null) {
            exploder.Explode();
        }

        return exploder;
    }

    private void handleRigidbodyCollision(RaycastHit hit) {
        Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
        if (rb != null) {
            Vector3 direction = (hit.collider.transform.position - transform.position).normalized * activeWeapon.stats.bulletForce;
            rb.AddForceAtPosition(direction, hit.collider.transform.position, ForceMode.Impulse);
        }
    }

    private EnemyController handleEnemyCollision(ref RaycastHit hit) {
        EnemyController enemyController = hit.collider.gameObject.GetComponent<EnemyController>();
        if (enemyController != null) {
            float playerDistance = Vector3.Distance(transform.position, hit.collider.transform.position);
            int effectiveDamage = CalcEffectiveDamage(playerDistance);
            enemyController.TakeDamage(effectiveDamage);
            GameObject bloodSplat = Instantiate(blood, hit.point, hit.collider.gameObject.transform.rotation);
            Destroy(bloodSplat, bloodTime);
        }

        return enemyController;
    }

    private void FireProjectile(RaycastHit hit) {
        Vector3 spawnPos = transform.position + (transform.forward * 2f);
        spawnPos.y += 0.5f;
        GameObject newProjectile = Instantiate(projectile, spawnPos, transform.rotation);
        newProjectile.transform.LookAt(hit.point);
        newProjectile.GetComponent<Rigidbody>().velocity = newProjectile.transform.forward * projectileSpeed;
    }

    private int CalcEffectiveDamage(float playerDistance) {
        int effectiveDamage = activeWeapon.stats.damage;

        if (playerDistance > activeWeapon.stats.effectiveRange) {
            int difference = Convert.ToInt32(activeWeapon.stats.range - playerDistance);
            effectiveDamage = Convert.ToInt32(activeWeapon.stats.damage * (difference / activeWeapon.stats.range));
        }

        return effectiveDamage;
    }

    private void updateCurrentWeapon() {
        for (int i = 0; i < weapons.Count; i++) {
            string weaponKey = (i + 1).ToString();
            if (Input.GetKeyDown(weaponKey) && weapons[i].collected) {
                activeWeapon.updateActiveWeapon(i);
                weaponController = FindObjectOfType<WeaponAnimationController>();
                currentWeaponState = weapons[i];
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
        yield return new WaitForSeconds(activeWeapon.stats.fireRate);
        shooting = false;
        weaponController.finishShoot();
    }
}
