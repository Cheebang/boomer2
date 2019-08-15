using UnityEngine;

[System.Serializable]
public class EnemyData {
    public bool dead;
    public string name;
    public int hp;

    public EnemyData(EnemyController enemy) {
        dead = enemy.dead;
        hp = enemy.hp;
        name = enemy.gameObject.name;
    }
}