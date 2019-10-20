using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyCollection : MonoBehaviour {
    public List<EnemyData> enemies { get; private set; } = new List<EnemyData>();

    private void Awake() {
        GameEvents.SaveInitiated += Save;
        GameEvents.LoadInitiated += Load;
    }

    void Save() {
        List<EnemyController> enemyControllers = new List<EnemyController>(FindObjectsOfType<EnemyController>());
        enemies = enemyControllers.Select(c => new EnemyData(c)).ToList<EnemyData>();
        SaveLoad.Save(enemies, "enemies");
    }

    void Load() {
        enemies = SaveLoad.Load<List<EnemyData>>("enemies");
    }
}
