using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class InteractionManager : MonoBehaviour {
    public float interactionDistance = 5f;
    private WeaponManager weaponManager;
    private HealthManager healthManager;
    private ItemManager itemManager;
    private HUDController hudController;
    private FirstPersonController player;
    private MainMenuController mainMenu;
    private bool paused = false;

    void Start() {
        weaponManager = GetComponent<WeaponManager>();
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
            GameObject[] gameObjects = (GameObject[])Resources.FindObjectsOfTypeAll(typeof(GameObject));
            GameEvents.OnSaveInitiated();
            if (paused) {
                paused = false;
                ContinueGame();
            }
        }

        if (Input.GetKeyDown(KeyCode.F3)) {
            hudController.Log("Quickloaded...");
            GameEvents.OnLoadInitiated();
        }

        CheckInteractions();
    }

    private void CheckInteractions() {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out hit, interactionDistance)) {
            Triggerable triggerable = hit.collider.gameObject.GetComponent<Triggerable>();
            if (triggerable != null) {
                triggerable.Interact();
            }
        }
    }

    void OnTriggerEnter(Collider collider) {
        Triggerable triggerable = collider.gameObject.GetComponent<Triggerable>();
        if (triggerable != null) {
            triggerable.PickUp();
        }
    }

    private void PauseGame() {
        player.paused = true;
        weaponManager.paused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0;
        mainMenu.Show();
    }
    private void ContinueGame() {
        player.paused = false;
        weaponManager.paused = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        Time.timeScale = 1;
        mainMenu.Hide();
    }
}
