using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour {
    public float interactionDistance = 5f;
    private FireWeapon fireWeapon;
    private HealthManager healthManager;

    void Start() {
        fireWeapon = GetComponent<FireWeapon>();
        healthManager = GetComponent<HealthManager>();
    }

    void Update() {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, interactionDistance)) {
            GameObject item = hit.collider.gameObject;
            if (hit.collider.CompareTag("Weapon")) {
                Debug.Log("picked up " + item.name);
                fireWeapon.PickUpWeapon(item.name);
                item.SetActive(false);
            }
            else if (hit.collider.CompareTag("Ammo")) {
                Debug.Log("picked up " + item.name);
                fireWeapon.PickUpAmmo(item.name);
                item.SetActive(false);
            }
            else if (hit.collider.CompareTag("Health")) {
                Debug.Log("picked up " + item.name);
                healthManager.PickUpHealth(item.name);
                item.SetActive(false);
            }
        }
    }
}
