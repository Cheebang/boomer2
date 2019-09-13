using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniqueId : MonoBehaviour {
    public static string Generate(GameObject gameObject) {
        return gameObject.transform.position.sqrMagnitude + "-" + gameObject.name + "-" + gameObject.transform.GetSiblingIndex();
    }
}
