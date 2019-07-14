using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour {
    public bool open = false;
    public string keyRequired = null;
    public int speed = 1;
    public Vector3 targetPos;

    private void Start() {
        targetPos = new Vector3(transform.position.x, transform.position.y + 5, transform.position.z);
    }

    public void AttemptOpenDoor(List<string> items) {
        if (!string.IsNullOrEmpty(keyRequired) && !items.Contains(keyRequired)) {
            Debug.Log("door requires " + keyRequired);
        }
        else {
            Debug.Log("door opens");
            open = !open;
        }
    }

    void Update() {
        if (open) {
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * speed);
        }
    }
}

