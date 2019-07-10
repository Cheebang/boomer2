using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour {
    public GameObject messagePanel;
    public Text messagePanelText;
    public Text ammoPanelText;
    public Text weaponNameText;
    public Text healthPanelText;

    private bool showMessagePanel = false;
    private FireWeapon weaponData;
    private HealthManager healthManager;

    void Start() {
        weaponData = FindObjectOfType<FireWeapon>();
        healthManager = FindObjectOfType<HealthManager>();
    }

    void Update() {
        weaponNameText.text = weaponData.currentWeapon.name;
        ammoPanelText.text = weaponData.currentWeapon.ammo.ToString();

        messagePanel.SetActive(showMessagePanel);
        if (showMessagePanel) {
            showMessagePanel = false;
        }

        healthPanelText.text = healthManager.health.ToString();
    }

    public void OpenMessagePanel(string text) {
        messagePanelText.text = text;
        showMessagePanel = true;
    }
}
