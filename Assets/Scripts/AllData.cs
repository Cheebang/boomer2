[System.Serializable]
public class AllData {
    public PlayerData playerData;
    public TriggerableData[] triggerableData;
    public EnemyData[] enemyData;

    public AllData(PlayerData playerData, TriggerableData[] triggerableData, EnemyData[] enemyData) {
        this.playerData = playerData;
        this.triggerableData = triggerableData;
        this.enemyData = enemyData;
    }
}
