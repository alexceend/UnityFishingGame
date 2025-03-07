using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SC_SellItems : MonoBehaviour
{

    public GameObject player;
    PlayerMovement playerMovement;

    public GameObject gameManager;
    GameManager gameManagerScript;

    public SC_InventorySystem inventorySystem;
    private FishCollection fishData;
    public TextAsset jsonFile; // Drag your JSON file here in the Unity Inspector

    private List<Fish> fishList = new List<Fish>();

    GameObject interactionText;
    GameObject xp_won_text;
    public Transform canvasTransform;


    Rigidbody rb;

    float player_x;
    float player_y;
    float player_z;

    float x;
    float y;
    float z;


    // Start is called before the first frame update
    void Start()
    {
        playerMovement = player.GetComponent<PlayerMovement>();

        fishData = JsonUtility.FromJson<FishCollection>(jsonFile.text);

        fishList.AddRange(fishData.common);
        fishList.AddRange(fishData.rare);
        fishList.AddRange(fishData.very_rare);

        rb = GetComponent<Rigidbody>();
        x = rb.position.x;
        y = rb.position.y;
        z = rb.position.z;

        gameManagerScript = gameManager.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {

        if (inRange())
        {
            if (interactionText == null)
            {
                DisplayText();
            }
        }
        else
        {
            if (interactionText != null)
            {
                Destroy(interactionText);
            }
        }

        if (inRange() && Input.GetKeyDown(KeyCode.F))
        {
            Sell();
            Destroy(interactionText);
            gameManagerScript.SavePlayerData();
        }
    }

    bool inRange()
    {

        if (playerMovement != null)
        {
            player_x = playerMovement.getPos().x;
            player_y = playerMovement.getPos().y;
            player_z = playerMovement.getPos().z;

            float distanceModule = Mathf.Sqrt(
                Mathf.Pow(player_x - x, 2) +
                Mathf.Pow(player_y - y, 2) +
                Mathf.Pow(player_z - z, 2));

            return distanceModule < 10;
        }
        return false;
    }

    void Sell()
    {

        int won_xp = 0;

        foreach (var item in inventorySystem.getInventoryItems())
        {
            foreach (var fish in fishList)
            {
                if(item.itemID == fish.id)
                {
                    gameManagerScript.addXPAndMoney(fish.xp_given, (fish.xp_given / 2));
                    won_xp += fish.xp_given;
                }
            }
        };

        DisplaySoldXp(won_xp);
        inventorySystem.Flush();
        StartCoroutine(DestroyTextDelay(xp_won_text, 2f));

    }

    private IEnumerator DestroyTextDelay(GameObject text, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(text);
    }


    void DisplayText()
    {
        interactionText = new GameObject("DynamicText");
        interactionText.transform.SetParent(canvasTransform, false);

        TextMeshProUGUI newText = interactionText.AddComponent<TextMeshProUGUI>();
        newText.text = "Press F to sell your items";
        newText.fontSize = 12;
        newText.color = Color.white;
        newText.alignment = TextAlignmentOptions.Right; // Align text to the right

        RectTransform rectTransform = newText.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(1, 1); // Top-right corner
        rectTransform.anchorMax = new Vector2(1, 1);
        rectTransform.pivot = new Vector2(1, 1); // Pivot to top-right
        rectTransform.anchoredPosition = new Vector2(-20, -20); // Offset from top-right
    }


    void DisplaySoldXp(int xp)
    {
        Debug.Log("Trace");
        xp_won_text = new GameObject("DynamicText");
        xp_won_text.transform.SetParent(canvasTransform, false);

        TextMeshProUGUI newText = xp_won_text.AddComponent<TextMeshProUGUI>();
        newText.text = "You won " + xp + " xp.";
        newText.fontSize = 24;
        newText.color = Color.white;
        newText.alignment = TextAlignmentOptions.Center; // Align text to the right

        RectTransform rectTransform = newText.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f); // Center anchor
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f); // Center pivot
        rectTransform.anchoredPosition = Vector2.zero; // Position at center
    }
}
