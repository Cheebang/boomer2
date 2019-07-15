using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class InteractionManager : MonoBehaviour {
    public float interactionDistance = 5f;
    private FireWeapon fireWeapon;
    private HealthManager healthManager;
    private ItemManager itemManager;
    private HUDController hudController;
    private FirstPersonController player;
    private bool paused = false;

    void Start() {
        fireWeapon = GetComponent<FireWeapon>();
        healthManager = GetComponent<HealthManager>();
        itemManager = GetComponent<ItemManager>();
        hudController = FindObjectOfType<HUDController>();
        player = GetComponent<FirstPersonController>();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            paused = !paused;
            player.paused = paused;
            if (!paused) {
                ContinueGame();
            }
        }

        if (paused) {
            PauseGame();
        }

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, interactionDistance)) {
            GameObject item = hit.collider.gameObject;
            if (hit.collider.CompareTag("Door")) {
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

    void OnTriggerEnter(Collider collider) {
        GameObject item = collider.gameObject;
        if (collider.CompareTag("Weapon")) {
            hudController.Log("picked up " + item.name);
            fireWeapon.PickUpWeapon(item.name);
            item.SetActive(false);
        }
        else if (collider.CompareTag("Ammo")) {
            hudController.Log("picked up " + item.name);
            fireWeapon.PickUpAmmo(item.name);
            item.SetActive(false);
        }
        else if (collider.CompareTag("Health")) {
            hudController.Log("picked up " + item.name);
            healthManager.PickUpHealth(item.name);
            item.SetActive(false);
        }
        else if (collider.CompareTag("Item")) {
            hudController.Log("picked up " + item.name);
            itemManager.PickUpItem(item.name);
            item.SetActive(false);
        }
    }

    private void PauseGame() {
        Time.timeScale = 0;
        hudController.OpenMessagePanel("Paused");
    }
    private void ContinueGame() {
        Time.timeScale = 1;
        Cursor.visible = false;
    }

}
