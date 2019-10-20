using UnityEngine;

[System.Serializable]
public class EnemyData {
    public bool dead;
    public string uniqueId;
    public float[] position;
    public int hp;

    public EnemyData(EnemyController enemy) {
        dead = enemy.dead;
        hp = enemy.hp;
        uniqueId = enemy.uniqueId;

        position = new float[3];
        position[0] = enemy.transform.position.x;
        position[1] = enemy.transform.position.y;
        position[2] = enemy.transform.position.z;
    }
}