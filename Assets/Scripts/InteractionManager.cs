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

        CheckInteractions();
    }

    private void CheckInteractions() {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, interactionDistance)) {
            Triggerable triggerable = hit.collider.gameObject.GetComponent<Triggerable>();
            if (triggerable != null) {
                triggerable.Interact();
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
        switch (collider.tag) {
            case "Weapon":
                fireWeapon.PickUpWeapon(item);
                break;
            case "Health":
                healthManager.PickUpHealth(item);
                break;
            case "Ammo":
                fireWeapon.PickUpAmmo(item);
                break;
            case "Armor":
                healthManager.PickUpArmor(item);
                break;
            case "Item":
                itemManager.PickUpItem(item);
                break;
            default:
                break;
        }
    }

    private void PauseGame() {
        player.paused = true;
        fireWeapon.paused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0;
        mainMenu.Show();
    }
    private void ContinueGame() {
        player.paused = false;
        fireWeapon.paused = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        Time.timeScale = 1;
        mainMenu.Hide();
    }
}
