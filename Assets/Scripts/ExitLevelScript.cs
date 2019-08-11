using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitLevelScript : Triggerable {
    private HUDController hudController;

    private void Start() {
        hudController = FindObjectOfType<HUDController>();
    }

    public override void Activate() {
        throw new System.NotImplementedException();
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
