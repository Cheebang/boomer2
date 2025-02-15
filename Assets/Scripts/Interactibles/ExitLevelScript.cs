﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitLevelScript : Triggerable {
    private HUDController hudController;

    private void Start() {
        hudController = FindObjectOfType<HUDController>();
    }

    public override void Interact() {
        if (Input.GetKeyDown(KeyCode.E)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else {
            hudController.OpenMessagePanel("Press E to exit level");
        }
    }
}
