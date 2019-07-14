using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour {
    public bool open = false;
    public string keyRequired = null;
    public int speed = 1;
    public Vector3 targetPos;
    private HUDController hud;

    private void Start() {
        targetPos = new Vector3(transform.position.x, transform.position.y + 5, transform.position.z);
        hud = FindObjectOfType<HUDController>();
    }

    public void AttemptOpenDoor(List<string> items) {
        bool doorRequiresKey = !string.IsNullOrEmpty(keyRequired);

        if (doorRequiresKey) {
            if (!items.Contains(keyRequired)) {
                hud.Log("door requires " + keyRequired);
                return;
            }
            hud.Log("door opened with " + keyRequired);
        }

        open = !open;
    }

    void Update() {
        if (open) {
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * speed);
        }
    }
}

