using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchScript : Triggerable {
    public GameObject door;
    public bool pushed = false;
    private HUDController hudController;

    private void Start() {
        hudController = FindObjectOfType<HUDController>();
    }

    public override void Activate() {
        throw new System.NotImplementedException();
    }

    public override void Interact() {
        if (!pushed) {
            hudController.OpenMessagePanel("Press E interact");
        }
        if (Input.GetKeyDown(KeyCode.E)) {
            PushSwitch();
        }
    }

    private void PushSwitch() {
        if (!pushed) {
            pushed = true;
            door.GetComponent<Triggerable>().Activate();
            FindObjectOfType<HUDController>().Log("a secret is revealed...");
        }
    }
}
