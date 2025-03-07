using TMPro;
using UnityEngine;

public class SC_InteractionToFish : MonoBehaviour
{

    public GameObject player;
    PlayerMovement playerMovement;


    public GameObject gameManager;
    GameManager gameManagerScript;

    Rigidbody rb;

    float player_x;
    float player_y;
    float player_z;

    float x;
    float y;
    float z;

    GameObject interactionText;
    public Transform canvasTransform;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        x = rb.position.x;
        y = rb.position.y;
        z = rb.position.z;

        playerMovement = player.GetComponent<PlayerMovement>();
        gameManagerScript = gameManager.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {

        if(inRange())
        {
            if(interactionText == null)
            {
                DisplayText();
            }
        }
        else
        {
            if(interactionText != null)
            {
                Destroy(interactionText);
            }
        }

        if (inRange() && Input.GetKeyDown(KeyCode.F) && !gameManagerScript.isFishing())
        {
            gameManagerScript.changeFishingState();
            playerMovement.canMove = false;
            Destroy(interactionText);
        }

        if(gameManagerScript.isFishing() && Input.GetKeyDown(KeyCode.Escape)){
            gameManagerScript.changeFishingState();
            Destroy(interactionText);
        }

        if (!gameManagerScript.isFishing())
        {
            playerMovement.canMove = true;
        }
    }

    bool inRange()
    {

        if(playerMovement != null)
        {
            player_x = playerMovement.getPos().x;
            player_y = playerMovement.getPos().y;
            player_z = playerMovement.getPos().z;

            float distanceModule = Mathf.Sqrt(
                Mathf.Pow(player_x - x, 2) +
                Mathf.Pow(player_y -y, 2) +
                Mathf.Pow(player_z - z, 2));

            return distanceModule < 10;
        }
        return false;
    }

    public bool getFishing()
    {
        return gameManagerScript.isFishing();
    }

    void DisplayText()
    {
        interactionText = new GameObject("DynamicText");
        interactionText.transform.SetParent(canvasTransform, false);

        TextMeshProUGUI newText = interactionText.AddComponent<TextMeshProUGUI>();
        newText.text = gameManagerScript.isFishing() ? "Pres ESC to stop fishing" : "Press F to start fishing";
        newText.fontSize = 12;
        newText.color = Color.white;
        newText.alignment = TextAlignmentOptions.Right; // Align text to the right

        RectTransform rectTransform = newText.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(1, 1); // Top-right corner
        rectTransform.anchorMax = new Vector2(1, 1);
        rectTransform.pivot = new Vector2(1, 1); // Pivot to top-right
        rectTransform.anchoredPosition = new Vector2(-20, -20); // Offset from top-right
    }

}
