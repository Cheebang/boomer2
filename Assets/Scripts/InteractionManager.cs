using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;

public class InteractionManager : MonoBehaviour {
    public float interactionDistance = 5f;
    private FireWeapon fireWeapon;
    private HealthManager healthManager;
    private ItemManager itemManager;
    private HUDController hudController;
    private FirstPersonController player;
    private MainMenuController mainMenu;
    private bool paused = false;

    void Start() {
        fireWeapon = GetComponent<FireWeapon>();
        healthManager = GetComponent<HealthManager>();
        itemManager = GetComponent<ItemManager>();
        hudController = FindObjectOfType<HUDController>();
        player = GetComponent<FirstPersonController>();
        mainMenu = FindObjectOfType<MainMenuController>();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            paused = !paused;
            player.paused = paused;
            fireWeapon.paused = paused;
            if (paused) {
                PauseGame();
            }
            else {
                ContinueGame();

            }
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
            else if (hit.collider.CompareTag("ExitLevel")) {
                if (Input.GetKeyDown(KeyCode.E)) {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                }
                else {
                    hudController.OpenMessagePanel("Press E to exit level");
                }
            }
            else if (hit.collider.CompareTag("Switch")) {
                SwitchScript switchScript = hit.collider.gameObject.GetComponent<SwitchScript>();

                if (!switchScript.pushed) {
                    hudController.OpenMessagePanel("Press E interact");
                }
                if (Input.GetKeyDown(KeyCode.E)) {
                    switchScript.PushSwitch();
                }
            }
        }
    }

    void OnTriggerEnter(Collider collider) {
        GameObject item = collider.gameObject;
        if (collider.CompareTag("Weapon")) {
            fireWeapon.PickUpWeapon(item);
        }
        else if (collider.CompareTag("Ammo")) {
            fireWeapon.PickUpAmmo(item);
        }
        else if (collider.CompareTag("Health")) {
            healthManager.PickUpHealth(item);
        }
        else if (collider.CompareTag("Item")) {
            hudController.Log("picked up " + item.name);
            itemManager.PickUpItem(item.name);
            item.SetActive(false);
        }
    }

    private void PauseGame() {
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        mainMenu.Show();
    }
    private void ContinueGame() {
        Time.timeScale = 1;
        Cursor.visible = false;
        mainMenu.Hide();
    }

}
