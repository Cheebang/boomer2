using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWeapon : MonoBehaviour {
    public Weapon currentWeapon;
    public AudioClip shootSound;
    public AudioClip emptySound;
    public GameObject bulletHole;
    public GameObject blood;

    public bool shooting = false;
    public int bulletHoleMax = 25;

    private float bloodTime = 0.5f;
    private AudioSource audioSource;
    private WeaponAnimationController weaponController;

    List<GameObject> bulletHoles = new List<GameObject>();
    private int currentBulletHoleIndex = 0;

    public List<Weapon> weapons = new List<Weapon>();

    void Start() {
        weapons.Add(new Pistol());
        weapons.Add(new Shotgun());
        weapons.Add(new MachineGun());
        currentWeapon = weapons[0];

        audioSource = GetComponent<AudioSource>();
        weaponController = FindObjectOfType<WeaponAnimationController>();
        for (int i = 0; i < bulletHoleMax; i++) {
            bulletHoles.Add(Instantiate(bulletHole));
        }
    }

    public void PickUpWeapon(string weaponName) {
        foreach (Weapon weapon in weapons) {
            if (weaponName == weapon.name) {
                if (!weapon.collected) {
                    weapon.collected = true;
                    weaponController.SwitchWeaponTo(currentWeapon.name, weapon.name);
                    currentWeapon = weapon;
                }
                else {
                    weapon.ammo = weapon.ammo + 20;
                }
            }
        }
    }

    public void PickUpAmmo(string ammoName) {
        foreach (Weapon weapon in weapons) {
            if (ammoName.Contains(weapon.name)) {
                weapon.ammo = weapon.ammo + 10;
            }
        }
    }

    void Update() {
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
                else if (hit.collider.gameObject.tag == "Enemy") {
                    EnemyController enemyController = hit.collider.gameObject.GetComponent<EnemyController>();
                    enemyController.TakeDamage(currentWeapon.damage);
                    GameObject bloodSplat = Instantiate(blood, hit.point, hit.collider.gameObject.transform.rotation);
                    StartCoroutine(SplatterBlood(bloodSplat));
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
