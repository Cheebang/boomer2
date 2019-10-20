using UnityEngine;

public class SwitchScript : Triggerable {
    public GameObject door;
    private HUDController hudController;

    private void Start() {
        base.Start();
        hudController = FindObjectOfType<HUDController>();
    }

    public override void SwitchState() {
        throw new System.NotImplementedException();
    }

    public override void Interact() {
        if (!interacted) {
            hudController.OpenMessagePanel("Press E interact");
        }
        if (Input.GetKeyDown(KeyCode.E)) {
            PushSwitch();
        }
    }

    private void PushSwitch() {
        if (!interacted) {
            interacted = true;
            door.GetComponent<Triggerable>().SwitchState();
            FindObjectOfType<HUDController>().Log("a secret is revealed...");
        }
    }
}
