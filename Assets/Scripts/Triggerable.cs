using UnityEngine;

public abstract class Triggerable : MonoBehaviour {
    public virtual void SwitchState() { }
    public virtual void Interact() { }
    public virtual void PickUp() { }
}
