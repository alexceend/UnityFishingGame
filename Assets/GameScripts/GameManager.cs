using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private PlayerData playerData;
    // Start is called before the first frame update
    void Start()
    {

        playerData = new PlayerData();

        playerData.isFishing = false;
        playerData.level = 0;
        playerData.xp = 0;
        playerData.levelMap = new List<KeyValuePair<int, int>>();

        playerData.seedLevelHasHmap();


        LoadPlayerData();
    }


    public void addXPAndMoney(int xp, float money)
    {
        playerData.addXPAndMoney(xp, money);
    }

    public void changeFishingState()
    {
        playerData.changeFishingState();
    }

    void LoadPlayerData()
    {
        string path = Application.persistentDataPath + "/playerData.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            playerData = JsonUtility.FromJson<PlayerData>(json);
        }
        else
        {
            // If no file exists, create default data
            playerData = new PlayerData { level = 0, xp = 0 };
            SavePlayerData();
        }

        // Example: Set player level and XP in your game
        Debug.Log("Player Level: " + playerData.level);
        Debug.Log("Player XP: " + playerData.xp);
    }

    public void SavePlayerData()
    {
        Debug.Log("Saved player data");
        string json = JsonUtility.ToJson(playerData);
        File.WriteAllText(Application.persistentDataPath + "/playerData.json", json);
    }

    public int getLevel()
    {
        return playerData.level;
    }

    public int getXP()
    {
        return playerData.xp;
    }

    public double getMoney()
    {
        return playerData.money;
    }

    public List<KeyValuePair<int, int>> getList()
    {
        if(playerData.levelMap == null)
        {
            playerData.levelMap = new List<KeyValuePair<int, int>>();
            playerData.seedLevelHasHmap();
        }
        return playerData.levelMap;
    }

    public bool isFishing()
    {
        return playerData.isFishing;
    }

    public void setFishing(bool state)
    {
        playerData.isFishing = state;
    }

    public void fixList()
    {
        playerData.levelMap = new List<KeyValuePair<int, int>>();
        playerData.seedLevelHasHmap();
    }

    public PlayerData GetPlayerData()
    {
        return playerData;
    }
}
