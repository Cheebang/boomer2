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

    private void Start() {
        newGameButton.onClick.AddListener(NewGame);
        saveButton.onClick.AddListener(Save);
        loadButton.onClick.AddListener(Load);
        quitButton.onClick.AddListener(QuitGame);
        isMainMenu = SceneManager.GetActiveScene().buildIndex == 0;
    }

    public void Show() {
        ui.SetActive(true);
    }

    public void Hide() {
        ui.SetActive(false);
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
