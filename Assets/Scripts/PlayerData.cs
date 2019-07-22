using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class PlayerData {
    public int health;
    public float[] position;
    public string[] items;
    public int currentLevel;

    public PlayerData(GameObject player) {
        HealthManager healthManager = player.GetComponent<HealthManager>();
        health = healthManager.health;

        position = new float[3];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;

        currentLevel = SceneManager.GetActiveScene().buildIndex;

        ItemManager itemManager = player.GetComponent<ItemManager>();
        items = itemManager.items.ToArray();
    }
}
