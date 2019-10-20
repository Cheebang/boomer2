using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class PlayerData {
    public float[] position;
    public float[] rotation;
    public int currentLevel;

    public PlayerData(GameObject player) {
        position = new float[3];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;

        rotation = new float[3];
        rotation[0] = player.transform.eulerAngles.x;
        rotation[1] = player.transform.eulerAngles.y;
        rotation[2] = player.transform.eulerAngles.z;

        currentLevel = SceneManager.GetActiveScene().buildIndex;
    }
}
