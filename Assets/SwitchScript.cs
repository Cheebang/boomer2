using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchScript : MonoBehaviour {
    public GameObject door;
    public bool pushed = false;

    public void PushSwitch() {
        if (!pushed) {
            pushed = true;
            door.GetComponent<DoorScript>().Unlock();
        }
    }
}
