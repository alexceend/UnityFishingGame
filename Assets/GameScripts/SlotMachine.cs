using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SlotMachine : MonoBehaviour
{
    float maxDistance = 10f;

    public Camera playerCam;
    private bool isLookingAtObject = false;

    public Image crossHair;

    public GameObject caseManagerOBJ;
    private CaseOpening CaseOpening;


    public GameObject GameManagerOBJ;
    private GameManager GameManager;

    private PlayerData pd;


    public GameObject textPrefab; // Assign your TMP prefab in the Inspector
    public Transform canvasTransform; // Assign your UI Canvas


    // Start is called before the first frame update
    void Start()
    {
        CaseOpening = caseManagerOBJ.GetComponent<CaseOpening>();
        GameManager = GameManagerOBJ.GetComponent<GameManager>();

        pd = GameManager.GetPlayerData();
    }

    // Update is called once per frame
    void Update()
    {

        isLookingAtObject = IsPlayerLookingAtObject();

        crossHair.enabled = isLookingAtObject;

        if (Input.GetMouseButtonUp(0) && isLookingAtObject)
        {
            OnInteract();
        }
    }

    bool IsPlayerLookingAtObject()
    {
        Ray ray = playerCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.red);

        if (Physics.Raycast(ray, out hit, maxDistance) && hit.collider.gameObject.transform == gameObject.transform.Find("slot"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void OnInteract()
    {
        if(pd.money >= 50)
        {
            int index = CaseOpening.StartCaseOpening();
            setMultiplier(index);
            pd.money -= 50;
            GameManager.SavePlayerData();

        } else {
            StartCoroutine(displayInsufficientMoneyText());
        }
    }

    public IEnumerator displayInsufficientMoneyText()
    {
        GameObject newText = Instantiate(textPrefab, canvasTransform);

        // Get the TMP component and set the text
        TextMeshProUGUI tmpComponent = newText.GetComponent<TextMeshProUGUI>();
        tmpComponent.text = "YOU NEED 50$ TO GAMBLE";

        // Center the text in the middle of the screen
        RectTransform rectTransform = newText.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = Vector2.zero; // (0,0) = Center of the canvas

        yield return new WaitForSeconds(2.0f);
        Destroy(newText);
    }

    void setMultiplier(int index)
    {
        switch (index)
        {
            case 0:
                fishing_rod_multiplier(1.5f, pd);
                break;
            case 1:
                moneyMultiplier(2f, pd);
                break;
            case 3:
                xpMultiplier(1.5f, pd);
                break;
            case 4:
                moneyMultiplier(5f, pd);
                break;
            case 5:
                xpMultiplier(5f, pd);
                break;
            case 6:
                fishing_rod_multiplier(2f, pd);
                break;
            case 7:
                moneyMultiplier(10f, pd);
                break;
            case 8:
                xpMultiplier(10f, pd);
                break;
            case 9:
                moneyMultiplier(0.5f, pd);
                break;
            case 10:
                fishing_rod_multiplier(2f, pd);
                break;
            case 11:
                moneyMultiplier(100f, pd);
                break;
            case 12:
                xpMultiplier(100f, pd);
                break;
            case 13:
                xpMultiplier(0.5f, pd);
                break;
            case 14:
                fishing_rod_multiplier(5f, pd);
                break;
            case 15:
                fishing_rod_multiplier(5f, pd);
                break;
        }
    }

    void moneyMultiplier(float mult, PlayerData pd)
    {
        pd.money_multiplier = mult;
    }

    void xpMultiplier(float mult, PlayerData pd)
    {
        pd.xp_multiplier = mult;
    }

    void fishing_rod_multiplier(float mult, PlayerData pd)
    {
        pd.fishing_rod_range_multiplier = mult;
    }

}
