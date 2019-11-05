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

    public void startWalking() {
        anim.SetBool("walking", true);
    }

    public void finishWalking() {
        anim.SetBool("walking", false);
    }

    internal void SwitchWeaponTo(string name, string newName) {
        anim.SetBool(name, false);
        anim.SetBool(newName, true);
    }
}
