using System.Collections.Generic;
using UnityEngine;

public class DoorScript : Triggerable {
    public string keyRequired = null;
    public int speed = 1;
    public Vector3 targetPos;
    public bool locked = false;

    private Vector3 startPos;
    private HUDController hud;

    private void Start() {
        base.Start();
        startPos = transform.position;
        targetPos = new Vector3(transform.position.x, transform.position.y + 5, transform.position.z);
        hud = FindObjectOfType<HUDController>();
    }

    void Update() {
        if (interacted) {
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * speed);
        }
        else {
            transform.position = Vector3.Lerp(transform.position, startPos, Time.deltaTime * speed);
        }
    }

    public override void Interact() {
        if (Input.GetKeyDown(KeyCode.E)) {
            AttemptOpen();
        }
        else {
            hud.OpenMessagePanel("Press E to open");
        }
    }

    private void AttemptOpen() {
        if (locked) {
            hud.Log("door is locked");
            return;
        }

        bool doorRequiresKey = !string.IsNullOrEmpty(keyRequired);
        if (doorRequiresKey) {
            List<string> items = FindObjectOfType<ItemManager>().items;
            if (!items.Contains(keyRequired)) {
                hud.Log("door requires " + keyRequired);
                return;
            }
            hud.Log("door opened with " + keyRequired);
        }

        SwitchState();
    }

    public override void SwitchState() {
        interacted = !interacted;
    }
}

