using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour {
    public float interactionDistance = 5f;
    private FireWeapon fireWeapon;
    private HealthManager healthManager;
    private ItemManager itemManager;
    private HUDController hudController;

    void Start() {
        fireWeapon = GetComponent<FireWeapon>();
        healthManager = GetComponent<HealthManager>();
        itemManager = GetComponent<ItemManager>();
        hudController = FindObjectOfType<HUDController>();
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
            else if (hit.collider.CompareTag("Item")) {
                Debug.Log("picked up " + item.name);
                itemManager.PickUpItem(item.name);
                item.SetActive(false);
            }
            else if (hit.collider.CompareTag("Door")) {
                if (Input.GetKeyDown(KeyCode.E)) {
                    Debug.Log("attempt to open door");
                    DoorScript doorScript = hit.collider.gameObject.GetComponent<DoorScript>();
                    doorScript.AttemptOpenDoor(itemManager.items);
                }
                else {
                    hudController.OpenMessagePanel("Press E to open");
                }
            }
        }
    }
}
