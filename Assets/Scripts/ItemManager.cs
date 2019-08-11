using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour {
    public List<string> items = new List<string>();
    private HUDController hudController;

    void Start() {
        hudController = FindObjectOfType<HUDController>();
    }

    internal void PickUpItem(GameObject item) {
        hudController.Log("picked up " + item.name);
        items.Add(item.name);
        item.SetActive(false);
    }
}
