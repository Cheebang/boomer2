using UnityEngine;

public abstract class Triggerable : MonoBehaviour {
    public virtual void SwitchState() { }
    public abstract void Interact();
}
