using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private Text collectibleText;

    [SerializeField]
    private Text shardText;

    [SerializeField]
    private Text ammoText;

    public bool isPaused;//When the variable is false, the game continues

    private int shardCount;
    private int collectibleCount;
    private int ammoCount = 3;

    [SerializeField]
    private List<PatrollingEnemy> patrollingEnemiesList = new List<PatrollingEnemy>();

    [SerializeField]
    private List<StationaryEnemy> stationaryEnemiesList = new List<StationaryEnemy>();

    [SerializeField]
    private List<CollectiblePickUp> collectiblePickUpList = new List<CollectiblePickUp>();

    [SerializeField]
    private List<ShardPickUp> shardPickUpList = new List<ShardPickUp>();

    [SerializeField]
    private List<AmmoPickUp> ammoPickUpList = new List<AmmoPickUp>();

    [SerializeField]
    private List<HealthPickUp> healthPickUpList = new List<HealthPickUp>();

    private static GameController gameControllerInstance;

    //private PlayerController player;
    //private Health health;
    private SaveAndLoadGame saveAndLoadGame;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        gameControllerInstance = this;
        collectibleCount = PlayerPrefs.GetInt("collectibles", 0);
        shardCount = PlayerPrefs.GetInt("shards", 0);
        ammoCount = PlayerPrefs.GetInt("ammo", 3);
        //player = FindObjectOfType<PlayerController>();
        //health = player.GetComponent<Health>();
        saveAndLoadGame = GetComponent<SaveAndLoadGame>();
    }

    //Update is called once per frame
    void Update()
    {
        collectibleText.text = collectibleCount.ToString();
        shardText.text = shardCount.ToString();
        ammoText.text = ammoCount.ToString();

        if(isPaused || SceneManager.GetActiveScene().buildIndex == 0 || SceneManager.GetActiveScene().buildIndex == 3)
        {
            Cursor.visible = true;
        }
        else
        {
            Cursor.visible = false;
        }
        // SaveGameInGame();
        // LoadGameInGame();
    }

    void OnDestroy()
    {
        //Debug.Log("GameController destroyed");
        PlayerPrefs.SetInt("collectibles", collectibleCount);
        PlayerPrefs.SetInt("shards", shardCount);
        PlayerPrefs.SetInt("ammo", ammoCount);
    }

    public void SaveGameInGame()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            saveAndLoadGame.SaveGame();
            //SaveGame();
        }
    }

    public void LoadGameInGame()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            //LoadGame();
            saveAndLoadGame.LoadGame();
            Debug.Log("Game Loaded");
        }
    }

    public void IncreaseShard()
    {
        shardCount++;
    }
    //public int GetShardCount()
    //{
    //    return this.shardCount;
    //}
    public void IncreaseCollectible()
    {
        collectibleCount++;
    }

    //public int GetCollectibleCount()
    //{
    //    return this.collectibleCount;
    //}

    public bool IncreaseAmmo()
    {
        ammoCount++;
        return true;
    }

    //public static int GetAmmoCount()
    //{
    //    return ammoCount;
    //}

    //public static void SetAmmoCount(int newAmmoCount)
    //{
    //    ammoCount = newAmmoCount;
    //}

    public static GameController GameControllerInstance { get => gameControllerInstance; }
    public List<PatrollingEnemy> PatrollingEnemiesList { get => patrollingEnemiesList; set => patrollingEnemiesList = value; }
    public List<StationaryEnemy> StationaryEnemiesList { get => stationaryEnemiesList; set => stationaryEnemiesList = value; }
    public List<CollectiblePickUp> CollectiblePickUpList { get => collectiblePickUpList; set => collectiblePickUpList = value; }
    public List<ShardPickUp> ShardPickUpList { get => shardPickUpList; set => shardPickUpList = value; }
    public List<AmmoPickUp> AmmoPickUpList { get => ammoPickUpList; set => ammoPickUpList = value; }
    public List<HealthPickUp> HealthPickUpList { get => healthPickUpList; set => healthPickUpList = value; }
    public int ShardCount { get => shardCount; set => shardCount = value; }
    public int CollectibleCount { get => collectibleCount; set => collectibleCount = value; }
    public int AmmoCount { get => ammoCount; set => ammoCount = value; }

}
