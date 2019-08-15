using UnityEngine;

public abstract class Triggerable : MonoBehaviour {
    public string objectName;

    public virtual void SwitchState() { }
    public virtual void Interact() { }
    public virtual void PickUp() { }
}
