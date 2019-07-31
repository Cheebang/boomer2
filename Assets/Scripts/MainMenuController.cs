using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {
    public Button newGameButton, saveButton, loadButton, quitButton;
    public GameObject ui;
    public bool isMainMenu;
    public GameObject player;

    public bool showUI;

    private void Start() {
        newGameButton.onClick.AddListener(NewGame);
        saveButton.onClick.AddListener(Save);
        loadButton.onClick.AddListener(Load);
        quitButton.onClick.AddListener(QuitGame);
        isMainMenu = SceneManager.GetActiveScene().buildIndex == 0;
    }

    void Update() {
        if (showUI) {
            ui.SetActive(true);
        }
        else {
            ui.SetActive(false);
        }
    }

    public void Show() {
        showUI = true;
    }

    public void Hide() {
        showUI = false;
    }

    void NewGame() {
        Debug.Log("clicked new game");
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }

    void Save() {
        Debug.Log("clicked save game");
        SaveSystem.SavePlayer(player);
    }

    void Load() {
        Debug.Log("clicked load game");
        PlayerData playerData = SaveSystem.LoadPlayer();
        player.GetComponent<InteractionManager>().LoadData(playerData);
    }

    void QuitGame() {
        Application.Quit();
    }
}
