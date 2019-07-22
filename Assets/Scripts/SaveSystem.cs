using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem {

    private static readonly string savePath = Application.persistentDataPath + "/player.data";

    public static void SavePlayer(GameObject player) {
        BinaryFormatter formatter = new BinaryFormatter();

        FileStream stream = new FileStream(savePath, FileMode.Create);
        PlayerData data = new PlayerData(player);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerData LoadPlayer() {
        if (File.Exists(savePath)) {
            BinaryFormatter formatter = new BinaryFormatter();

            FileStream stream = new FileStream(savePath, FileMode.Open);
            PlayerData data = (PlayerData)formatter.Deserialize(stream);
            stream.Close();

            return data;
        }
        else {
            Debug.Log("no save data found");
            return null;
        }
    }
}
