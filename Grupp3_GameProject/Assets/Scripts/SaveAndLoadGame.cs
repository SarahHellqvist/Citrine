using System.IO;
using System.Xml;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SaveAndLoadGame : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerController player;
    [SerializeField] private Health health;

    private static bool reload;

    // Start is called before the first frame update
    void Start()
    {
        if(player == null)
        {
            player = FindObjectOfType<PlayerController>();
        }
        if(health == null)
        {
            health = player.GetComponent<Health>();  
        }
        if(reload)
        {
            reload = false;
            LoadGame();
        }
    }

    public void SaveGame()
    {
        Save save = CreateSaveGameObject();
        XmlDocument xmlDocument = new XmlDocument();

        #region CreateXML elements

        //XmlElement : one of the most common nodes
        XmlElement root = xmlDocument.CreateElement("Save");//<Save>...elements...</Save>
        root.SetAttribute("FileName", "File_01");

        SaveXMLObject(xmlDocument, root, "SceneNum", save.sceneNum.ToString());

        //XmlElement.Innertext: Gets or sets the concatenated values of the node and all its children
        // XmlElement collectibleNumElement = xmlDocument.CreateElement("CollectibleNum");//<CollectibleNum>Game Collectibles detail data</CollectibleNum> under Root
        // collectibleNumElement.InnerText = save.collectiblesNum.ToString();
        // root.AppendChild(collectibleNumElement);//Append inside the SAVE braces (AS a CHILD / Element)
        SaveXMLObject(xmlDocument, root, "CollectibleNum", save.collectiblesNum.ToString());
                                                //AppendChild: Adds the specified node to the end of the list of child nodes, of this node. 

        // XmlElement shardNumElement = xmlDocument.CreateElement("ShardNum");//<ShardNum> ... </ShardNum> Under Root
        // shardNumElement.InnerText = save.shardsNum.ToString();
        // root.AppendChild(shardNumElement);//Append inside the SAVE braces (AS a CHILD / Element)
        SaveXMLObject(xmlDocument, root, "ShardNum", save.shardsNum.ToString());

        // XmlElement ammoNumElement = xmlDocument.CreateElement("AmmoNum");//<AmmoNum> ... </AmmoNum> Under Root
        // ammoNumElement.InnerText = save.ammoNum.ToString();
        // root.AppendChild(ammoNumElement);//Append inside the SAVE braces (AS a CHILD / Element)
        SaveXMLObject(xmlDocument, root, "AmmoNum", save.ammoNum.ToString());

        // XmlElement playerPosXElement = xmlDocument.CreateElement("PlayerPositionX");
        // playerPosXElement.InnerText = save.playerPositionX.ToString();
        // root.AppendChild(playerPosXElement);
        SaveXMLObject(xmlDocument, root, "PlayerPositionX", save.playerPositionX.ToString());

        // XmlElement playerPosYElement = xmlDocument.CreateElement("PlayerPositionY");
        // playerPosYElement.InnerText = save.playerPositionY.ToString();
        // root.AppendChild(playerPosYElement);
        SaveXMLObject(xmlDocument, root, "PlayerPositionY", save.playerPositionY.ToString());

        // XmlElement playerPosZElement = xmlDocument.CreateElement("PlayerPositionZ");
        // playerPosZElement.InnerText = save.playerPositionZ.ToString();
        // root.AppendChild(playerPosZElement);
        SaveXMLObject(xmlDocument, root, "PlayerPositionZ", save.playerPositionZ.ToString());

        // XmlElement playerHealthNumElement = xmlDocument.CreateElement("PlayerHealthNum");
        // playerHealthNumElement.InnerText = save.playerHealth.ToString();
        // root.AppendChild(playerHealthNumElement);
        SaveXMLObject(xmlDocument, root, "PlayerHealthNum", save.playerHealth.ToString());

        //SAVE ENEMIES(patrollingEnemy) position and their status  
        SaveXML(xmlDocument, root, "PatrollingEnemy", save.patrollingEnemyPositionX, save.patrollingEnemyPositionY, save.patrollingEnemyPositionZ, save.patrollingEnemyIsActive);
        
        //SAVE ENEMIES(StationaryEnemy) position and their status  
        SaveXML(xmlDocument, root, "StationaryEnemy", save.stationaryEnemyPositionX, save.stationaryEnemyPositionY, save.stationaryEnemyPositionZ, save.stationaryEnemyIsActive);
        
        //SAVE CollectiblePickUps position and their status
        SaveXML(xmlDocument, root, "Collectible", save.collectiblePositionX, save.collectiblePositionY, save.collectiblePositionZ, save.collectibleIsActive);

        //SAVE ShardPickUps position and their status
        SaveXML(xmlDocument, root, "Shard", save.shardPositionX, save.shardPositionY, save.shardPositionZ, save.shardIsActive);

        //SAVE AmmoPickUps position and their status
        SaveXML(xmlDocument, root, "Ammo", save.ammoPositionX, save.ammoPositionY, save.ammoPositionZ, save.ammoIsActive);

        //SAVE HealthPickUps position and their status
        SaveXML(xmlDocument, root, "Health", save.healthPositionX, save.healthPositionY, save.healthPositionZ, save.healthIsActive);

        #endregion

        xmlDocument.AppendChild(root);//Add the root and its children elements to the XML Document

        xmlDocument.Save(Application.dataPath + "/DataXML.text");
        if (File.Exists(Application.dataPath + "/DataXML.text"))
        {
            //Debug.Log("XML FILE SAVED");
        }
    }

    private void SaveXMLObject(XmlDocument xml, XmlElement containerElement, string elementName, string innerText)
    {
        XmlElement element = xml.CreateElement(elementName);
        element.InnerText = innerText;
        containerElement.AppendChild(element);
    }
    private void SaveXML(XmlDocument xml, XmlElement root, string containerName, List<float> posX, List<float> posY, List<float> posZ, List<bool> isActive)
    {
        for(int i = 0; i < posX.Count; i++)
        {
            XmlElement containerElement = xml.CreateElement(containerName);
            SaveXMLObject(xml, containerElement, containerName + "PositionX", posX[i].ToString());
            SaveXMLObject(xml, containerElement, containerName + "PositionY", posY[i].ToString());
            SaveXMLObject(xml, containerElement, containerName + "PositionZ", posZ[i].ToString());
            SaveXMLObject(xml, containerElement, containerName + "IsActive", isActive[i].ToString());
            root.AppendChild(containerElement);
        }
    }

    private int LoadXMLInt(XmlDocument xml, string elementName, int i)
    {
        XmlNodeList element = xml.GetElementsByTagName(elementName);
        int elementValue = int.Parse(element[i].InnerText);
        return elementValue;
    }

    private float LoadXMLFloat(XmlDocument xml, string elementName, int i)
    {
        XmlNodeList element = xml.GetElementsByTagName(elementName);
        float elementValue = float.Parse(element[i].InnerText);
        return elementValue;
    }

    private bool LoadXMLBool(XmlDocument xml, string elementName, int i)
    {
        XmlNodeList element = xml.GetElementsByTagName(elementName);
        bool elementValue = bool.Parse(element[i].InnerText);
        return elementValue;
    }

    private void LoadXML(string containerName, XmlDocument xml, List<float> posX, List<float> posY, List<float> posZ, List<bool> isActive)
    {
         XmlNodeList container = xml.GetElementsByTagName(containerName); //XmlNodeList: Represents an ordered collection of nodes.
        if (container.Count != 0)
        {
            for (int i = 0; i < container.Count; i++)
            {
                posX.Add(LoadXMLFloat(xml, containerName + "PositionX", i));
                posY.Add(LoadXMLFloat(xml, containerName + "PositionY", i));
                posZ.Add(LoadXMLFloat(xml, containerName + "PositionZ", i));
                isActive.Add(LoadXMLBool(xml, containerName + "IsActive", i));
            }
        }
    }

    public void LoadGame()
    {
        if (File.Exists(Application.dataPath + "/DataXML.text"))
        {
            //LOAD THE GAME
            Save save = new Save();

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(Application.dataPath + "/DataXML.text");

            save.sceneNum = LoadXMLInt(xmlDocument, "SceneNum", 0);
            
            if(save.sceneNum != SceneManager.GetActiveScene().buildIndex)
            {
                reload = true;
                SceneManager.LoadScene(save.sceneNum);
                return;
            }

            //Get the SAVE FILE DATA from the FILE
            save.collectiblesNum = LoadXMLInt(xmlDocument, "CollectibleNum", 0);

            save.shardsNum  = LoadXMLInt(xmlDocument, "ShardNum", 0);

            save.ammoNum = LoadXMLInt(xmlDocument, "AmmoNum", 0);

            save.playerPositionX = LoadXMLFloat(xmlDocument, "PlayerPositionX", 0);

            save.playerPositionY = LoadXMLFloat(xmlDocument, "PlayerPositionY", 0);

            save.playerPositionZ = LoadXMLFloat(xmlDocument, "PlayerPositionZ", 0);

            save.playerHealth = LoadXMLFloat(xmlDocument, "PlayerHealthNum", 0);

            //LOAD PatrollingEnemies positions and their status
            LoadXML("PatrollingEnemy", xmlDocument, save.patrollingEnemyPositionX, save.patrollingEnemyPositionY, save.patrollingEnemyPositionZ, save.patrollingEnemyIsActive);

            //LOAD StationaryEnemies positions and their status
            LoadXML("StationaryEnemy", xmlDocument, save.stationaryEnemyPositionX, save.stationaryEnemyPositionY, save.stationaryEnemyPositionZ, save.stationaryEnemyIsActive);

            //LOAD CollectilblePickUps positions and their status
            LoadXML("Collectible", xmlDocument, save.collectiblePositionX, save.collectiblePositionY, save.collectiblePositionZ, save.collectibleIsActive);

            //LOAD ShardPicUps positions and their status
            LoadXML("Shard", xmlDocument, save.shardPositionX, save.shardPositionY, save.shardPositionZ, save.shardIsActive);

            //LOAD AmmoPickUps positions and their status
            LoadXML("Ammo", xmlDocument, save.ammoPositionX, save.ammoPositionY, save.ammoPositionZ, save.ammoIsActive);

            //LOAD HealthPickUps positions and their status
            LoadXML("Health", xmlDocument, save.healthPositionX, save.healthPositionY, save.healthPositionZ, save.healthIsActive);

            //ASSIGN the save data to the game real data such as collectibles, shards and player Position, etc
            GameController.GameControllerInstance.CollectibleCount = save.collectiblesNum;
            GameController.GameControllerInstance.ShardCount = save.shardsNum;
            GameController.GameControllerInstance.AmmoCount = save.ammoNum;
            player.transform.position = new Vector3(save.playerPositionX, save.playerPositionY, save.playerPositionZ);
            health.SetCurrentHealth(save.playerHealth);

            //PatrollingEnemy position
            ResetEntityPosition<PatrollingEnemy>(save.patrollingEnemyPositionX, save.patrollingEnemyPositionY, save.patrollingEnemyPositionZ, save.patrollingEnemyIsActive, GameController.GameControllerInstance.PatrollingEnemiesList);

            //StationaryEnemy position
            ResetEntityPosition<StationaryEnemy>(save.stationaryEnemyPositionX, save.stationaryEnemyPositionY, save.stationaryEnemyPositionZ, save.stationaryEnemyIsActive, GameController.GameControllerInstance.StationaryEnemiesList);

            //CollectiblePickUp position
            ResetEntityPosition<CollectiblePickUp>(save.collectiblePositionX, save.collectiblePositionY, save.collectiblePositionZ, save.collectibleIsActive, GameController.GameControllerInstance.CollectiblePickUpList);

            //ShardPickUp position
            ResetEntityPosition<ShardPickUp>(save.shardPositionX, save.shardPositionY, save.shardPositionZ, save.shardIsActive, GameController.GameControllerInstance.ShardPickUpList);

            //AmmoPickUp position
            ResetEntityPosition<AmmoPickUp>(save.ammoPositionX, save.ammoPositionY, save.ammoPositionZ, save.ammoIsActive, GameController.GameControllerInstance.AmmoPickUpList);

            // HealthPickUp position
            ResetEntityPosition<HealthPickUp>( save.healthPositionX,  save.healthPositionY,  save.healthPositionZ,  save.healthIsActive, GameController.GameControllerInstance.HealthPickUpList);
        }
        else
        {
            //Debug.Log("NOT FOUNDED FILE");
        }
    }

    private void ResetEntityPosition<T>(List<float> posX, List<float> posY, List<float> posZ, List<bool> isActive, List<T> objectList) where T : Component
    {
        for(int i = 0; i < isActive.Count ; i++)
        {
            if(objectList[i].gameObject.activeSelf == false) 
            {
                if(isActive[i] == false) //If the object is not active, but was active before we pressed the save button
                {
                    objectList[i].gameObject.SetActive(true);
                }
            }
            else
            {
                if (isActive[i] == true) //If the object is active, but was not active before we pressed the save button
                {
                    objectList[i].gameObject.SetActive(false);
                }
                SetPosition(posX[i], posY[i], posZ[i], isActive[i], objectList[i].transform);
            }
        }
    }

    private void SetPosition(float posX, float posY, float posZ, bool isActive, Transform trans)
    {
        Vector3 savedPosition = new Vector3(posX, posY, posZ);
        trans.position = savedPosition;
    }

    private Save CreateSaveGameObject()
    {
        Save save = new Save();

        save.sceneNum = SceneManager.GetActiveScene().buildIndex;
        save.collectiblesNum = GameController.GameControllerInstance.CollectibleCount;
        save.shardsNum = GameController.GameControllerInstance.ShardCount;
        save.ammoNum = GameController.GameControllerInstance.AmmoCount;

        save.playerPositionX = player.transform.position.x;
        save.playerPositionY = player.transform.position.y;
        save.playerPositionZ = player.transform.position.z;
        save.playerHealth = health.GetCurrentHealth();

        //PatrollingEnemy position
        foreach (PatrollingEnemy patrollingEnemy in GameController.GameControllerInstance.PatrollingEnemiesList)
        {
            save.patrollingEnemyIsActive.Add(patrollingEnemy.PatrollingEnemyIsDead());
            save.patrollingEnemyPositionX.Add(patrollingEnemy.GetPatrollingEnemyPosX());
            save.patrollingEnemyPositionY.Add(patrollingEnemy.GetPatrollingEnemyPosY());
            save.patrollingEnemyPositionZ.Add(patrollingEnemy.GetPatrollingEnemyPosZ());
        }

        //StationaryEnemy position
        foreach (StationaryEnemy stationaryEnemy in GameController.GameControllerInstance.StationaryEnemiesList)
        {
            save.stationaryEnemyIsActive.Add(stationaryEnemy.StationaryEnemyIsDead());
            save.stationaryEnemyPositionX.Add(stationaryEnemy.GetStationaryEnemyPosX());
            save.stationaryEnemyPositionY.Add(stationaryEnemy.GetStationaryEnemyPosY());
            save.stationaryEnemyPositionZ.Add(stationaryEnemy.GetStationaryEnemyPosZ());
        }

        //CollectiblePickUp position
        foreach (CollectiblePickUp collectiblePickUp in GameController.GameControllerInstance.CollectiblePickUpList)
        {
            save.collectibleIsActive.Add(collectiblePickUp.CollectibleIsPickedUp());
            save.collectiblePositionX.Add(collectiblePickUp.GetCollectiblePickUpPosX());
            save.collectiblePositionY.Add(collectiblePickUp.GetCollectiblePickUpPosY());
            save.collectiblePositionZ.Add(collectiblePickUp.GetCollectiblePickUpPosZ());
        }

        //ShardPickUp position
        foreach (ShardPickUp shardPickUp in GameController.GameControllerInstance.ShardPickUpList)
        {
            save.shardIsActive.Add(shardPickUp.ShardIsPickedUp());
            save.shardPositionX.Add(shardPickUp.GetShardPickUpPosX());
            save.shardPositionY.Add(shardPickUp.GetShardPickUpPosY());
            save.shardPositionZ.Add(shardPickUp.GetShardPickUpPosZ());
        }

        //AmmoPickUp position
        foreach (AmmoPickUp ammoPickUp in GameController.GameControllerInstance.AmmoPickUpList)
        {
            save.ammoIsActive.Add(ammoPickUp.AmmoIsPickedUp());
            save.ammoPositionX.Add(ammoPickUp.GetAmmoPickUpPosX());
            save.ammoPositionY.Add(ammoPickUp.GetAmmoPickUpPosY());
            save.ammoPositionZ.Add(ammoPickUp.GetAmmoPickUpPosZ());
        }

        //HealthPickUp position
        foreach (HealthPickUp healthPickUp in GameController.GameControllerInstance.HealthPickUpList)
        {
            save.healthIsActive.Add(healthPickUp.HealthIsPickedUp());
            save.healthPositionX.Add(healthPickUp.GetHealthPickUpPosX());
            save.healthPositionY.Add(healthPickUp.GetHealthPickUpPosY());
            save.healthPositionZ.Add(healthPickUp.GetHealthPickUpPosZ());
        }

        return save;
    }
}
