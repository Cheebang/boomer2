using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {
    public Button newGameButton, quitButton;
    public GameObject ui;

    private void Start() {
        newGameButton.onClick.AddListener(NewGame);
        quitButton.onClick.AddListener(QuitGame);
    }

    public void Show() {
        ui.SetActive(true);
    }

    public void Hide() {
        ui.SetActive(false);
    }

    void NewGame() {
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }

    void QuitGame() {
        Application.Quit();
    }
}
