using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour {
    public List<string> items = new List<string>();

    internal void PickUpItem(string name) {
        items.Add(name);
    }
}
