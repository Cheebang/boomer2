using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour {
    public GameObject messagePanel;
    public Text messagePanelText;
    public Text ammoPanelText;
    public Text weaponNameText;
    public Text healthPanelText;
    public Text log;
    public float logTextDisplayLength = 3f;

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

    public void Log(string newText) {
        newText = newText.ToUpper();
        if (string.IsNullOrEmpty(log.text)) {
            log.text = newText;
        }
        else {
            log.text = newText + "\n" + log.text;
        }
        StartCoroutine(ClearText());
    }

    IEnumerator ClearText() {
        yield return new WaitForSeconds(logTextDisplayLength);

        string[] logMessages = log.text.Split('\n');
        string newLog = "";
        if (logMessages.Count() > 1) {
            Array.Resize(ref logMessages, logMessages.Length - 1);
            newLog = string.Join("\n", logMessages);
        }
        log.text = newLog;
    }
}
