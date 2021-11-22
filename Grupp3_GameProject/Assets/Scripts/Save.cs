using System.Collections.Generic;

//THIS CLASS is used for us to combine all elements needed to saved together
[System.Serializable]
public class Save
{
    public int sceneNum;
    public int collectiblesNum;
    public int shardsNum;
    public int ammoNum;

    public float playerPositionX;
    public float playerPositionY;
    public float playerPositionZ;
    public float playerHealth;

    //Saves the patrolling enemies position 
    public List<float> patrollingEnemyPositionX = new List<float>();
    public List<float> patrollingEnemyPositionY = new List<float>();
    public List<float> patrollingEnemyPositionZ = new List<float>();
    public List<bool> patrollingEnemyIsActive = new List<bool>();

    //Saves the stationary enemies position 
    public List<float> stationaryEnemyPositionX = new List<float>();
    public List<float> stationaryEnemyPositionY = new List<float>();
    public List<float> stationaryEnemyPositionZ = new List<float>();
    public List<bool> stationaryEnemyIsActive = new List<bool>();

    //Saves the collectibles position
    public List<float> collectiblePositionX = new List<float>();
    public List<float> collectiblePositionY = new List<float>();
    public List<float> collectiblePositionZ = new List<float>();
    public List<bool> collectibleIsActive = new List<bool>();

    //Saves the shards position
    public List<float> shardPositionX = new List<float>();
    public List<float> shardPositionY = new List<float>();
    public List<float> shardPositionZ = new List<float>();
    public List<bool> shardIsActive = new List<bool>();

    //Saves the ammos position
    public List<float> ammoPositionX = new List<float>();
    public List<float> ammoPositionY = new List<float>();
    public List<float> ammoPositionZ = new List<float>();
    public List<bool> ammoIsActive = new List<bool>();

    //Saves the ammos position
    public List<float> healthPositionX = new List<float>();
    public List<float> healthPositionY = new List<float>();
    public List<float> healthPositionZ = new List<float>();
    public List<bool> healthIsActive = new List<bool>();


    //Enemy Health Point
    //public List<int> enemyHps = new List<int>();

}
