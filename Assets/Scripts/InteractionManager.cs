using System;
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
        }

        if (paused) {
            PauseGame();
        }
        else {
            ContinueGame();

        }

        if (Input.GetKeyDown(KeyCode.F2)) {
            hudController.Log("Quicksaved...");
            SaveSystem.SavePlayer(gameObject);
            if (paused) {
                paused = false;
                ContinueGame();
            }
        }

        if (Input.GetKeyDown(KeyCode.F3)) {
            hudController.Log("Quickloaded...");
            PlayerData playerData = SaveSystem.LoadPlayer();
            LoadData(playerData);
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

    internal void LoadData(PlayerData playerData) {
        player.transform.position = new Vector3(playerData.position[0], playerData.position[1], playerData.position[2]);
        healthManager.health = playerData.health;
        itemManager.items = new List<string>(playerData.items);
        if (paused) {
            paused = false;
            ContinueGame();
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
        player.paused = true;
        fireWeapon.paused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        player.m_MouseLook.lockCursor = false;
        player.m_MouseLook.SetCursorLock(false);
        player.m_MouseLook.UpdateCursorLock();
        Time.timeScale = 0;
        mainMenu.Show();
    }
    private void ContinueGame() {
        player.paused = false;
        fireWeapon.paused = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        player.m_MouseLook.lockCursor = true;
        player.m_MouseLook.SetCursorLock(true);
        player.m_MouseLook.UpdateCursorLock();
        Time.timeScale = 1;
        mainMenu.Hide();
    }

}
