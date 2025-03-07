using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EncyclopediaManager : MonoBehaviour
{
    public TextAsset jsonFile; // Assign this in the Unity Inspector
    private EncyclopediaCollection encyclopediaData;
    private string jsonFilePath;

    void Awake()
    {
        jsonFilePath = Path.Combine(Application.persistentDataPath, "FishEncyclopedia.json");

        if (File.Exists(jsonFilePath))
        {
            string jsonText = File.ReadAllText(jsonFilePath);
            encyclopediaData = JsonUtility.FromJson<EncyclopediaCollection>(jsonText);
        }
        else
        {
            encyclopediaData = JsonUtility.FromJson<EncyclopediaCollection>(jsonFile.text);
            SaveJson();
        }
    }

    public void SaveJson()
    {
        string updatedJson = JsonUtility.ToJson(encyclopediaData, true);
        File.WriteAllText(jsonFilePath, updatedJson);
        Debug.Log("Encyclopedia saved.");
    }

    public void MarkFishAsCaught(int id)
    {
        foreach (var entry in encyclopediaData.encyclopedia)
        {
            if (entry.id == id)
            {
                entry.caught = true;
                SaveJson();
                Debug.Log("Fish ID " + id + " marked as caught!");
                return;
            }
        }
    }



    public bool IsFishCaught(int id)
    {
        foreach (var entry in encyclopediaData.encyclopedia)
        {
            if (entry.id == id)
            {
                return entry.caught;
            }
        }
        return false; // If fish is not found, assume it's not caught
    }

    public EncyclopediaCollection GetEncyclopediaCollection()
    {
        if (encyclopediaData == null)
        {
            Debug.LogWarning("Encyclopedia data was NULL. Creating a new empty collection.");
            encyclopediaData = new EncyclopediaCollection { encyclopedia = new List<Encyclopedia>() };
        }
        return encyclopediaData;
    }

    public void ResetEncyclopedia()
    {
        if (jsonFile == null)
        {
            Debug.LogError("No default JSON file assigned! Cannot reset encyclopedia.");
            return;
        }

        // Overwrite the saved file with the original JSON data
        File.WriteAllText(jsonFilePath, jsonFile.text);
        Debug.Log("Encyclopedia reset to default.");

        // Reload the encyclopedia data
        encyclopediaData = JsonUtility.FromJson<EncyclopediaCollection>(jsonFile.text);
    }

}
