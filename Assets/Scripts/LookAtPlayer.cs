using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class LookAtPlayer : MonoBehaviour {
    private FirstPersonController player;

    void Start() {
        player = FindObjectOfType<FirstPersonController>();
    }

    void Update() {
        Vector3 v = new Vector3(0, player.transform.position.y - transform.position.y, 0);
        transform.LookAt(player.transform.position - v);
    }
}
