using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public static class SaveSystem {

    private static readonly string savePath = Application.persistentDataPath + "/player.data";

    public static void Save(GameObject player, GameObject[] gameObjects) {
        BinaryFormatter formatter = new BinaryFormatter();

        FileStream stream = new FileStream(savePath, FileMode.Create);
        PlayerData playerData = new PlayerData(player);

        List<TriggerableData> tds = new List<TriggerableData>();
        List<EnemyData> eds = new List<EnemyData>();

        foreach (GameObject gameObject in gameObjects) {
            Triggerable triggerable = gameObject.GetComponent<Triggerable>();
            if (triggerable != null) {
                TriggerableData td = new TriggerableData(triggerable);
                tds.Add(td);
            }
            EnemyController enemy = gameObject.GetComponent<EnemyController>();
            if (enemy != null) {
                EnemyData ed = new EnemyData(enemy);
                eds.Add(ed);
            }
        }

        AllData allData = new AllData(playerData, tds.ToArray(), eds.ToArray());

        formatter.Serialize(stream, allData);
        stream.Close();
    }

    public static PlayerData Load(GameObject[] gameObjects) {
        if (File.Exists(savePath)) {
            BinaryFormatter formatter = new BinaryFormatter();

            FileStream stream = new FileStream(savePath, FileMode.Open);
            AllData data = (AllData)formatter.Deserialize(stream);
            stream.Close();

            //SceneManager.LoadScene(data.playerData.currentLevel);

            List<TriggerableData> tds = new List<TriggerableData>(data.triggerableData);
            List<EnemyData> eds = new List<EnemyData>(data.enemyData);

            foreach (GameObject gameObject in gameObjects) {
                Triggerable triggerable = gameObject.GetComponent<Triggerable>();
                if (triggerable != null) {
                    TriggerableData td = tds.Find(o => o.name == gameObject.name);
                    gameObject.SetActive(td.isActive);
                }

                EnemyController enemy = gameObject.GetComponent<EnemyController>();
                if (enemy != null) {
                    EnemyData ed = eds.Find(o => o.name == gameObject.name);
                    enemy.hp = ed.hp;
                    if (ed.dead) {
                        enemy.Die();
                    }
                    else {
                        enemy.Reset();
                    }
                }
            }

            return data.playerData;
        }
        else {
            Debug.Log("no save data found");
            return null;
        }
    }
}
