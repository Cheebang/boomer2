using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour {
    public GameObject messagePanel;
    public Text messagePanelText;
    public Text ammoPanelText;
    public Text weaponNameText;
    private bool show = false;
    private FireWeapon weaponData;

    void Start() {
        weaponData = FindObjectOfType<FireWeapon>();
    }

    void Update() {
        weaponNameText.text = weaponData.currentWeapon.name;
        ammoPanelText.text = weaponData.currentWeapon.ammo.ToString();
        messagePanel.SetActive(show);
        if (show) {
            show = false;
        }
    }

    public void OpenMessagePanel(string text) {
        messagePanelText.text = text;
        show = true;
    }
}
