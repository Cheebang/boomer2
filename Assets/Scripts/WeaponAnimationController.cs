using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimationController : MonoBehaviour {
    private Animator anim;

    void Start() {
        anim = GetComponent<Animator>();
    }

    public void shoot() {
        anim.SetBool("shooting", true);
    }

    public void finishShoot() {
        anim.SetBool("shooting", false);
    }

    internal void SwitchWeaponTo(string name, string newName) {
        anim.SetBool(name, false);
        anim.SetBool(newName, true);
    }
}
