using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class EncyclopediaSystem : MonoBehaviour
{

    public GameObject inventoryUI;  // UI Panel
    public Transform inventoryGrid; // Grid Layout Group
    public GameObject inventorySlotPrefab; // Slot Prefab

    private bool encyclopediaOpen = false;
    private List<SC_Item> items = new List<SC_Item>();

    public List<SC_Item> allFishItems; // Assign all SC_Item assets in the Inspector
    public GameObject encyclopediaManager;
    private EncyclopediaCollection encyclopediaCollection;

    GameObject interactionText;
    public Transform canvasTransform;


    void Start()
    {

        if (inventoryUI == null)
        {
            Debug.LogError("inventoryUI is not assigned in SC_InventorySystem! Assign it in the Inspector.");
            return;
        }


        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        inventoryUI.SetActive(false); // Hide inventory at start

        encyclopediaCollection = encyclopediaManager.GetComponent<EncyclopediaManager>().GetEncyclopediaCollection();
    }

    void Update()
    {
        if(encyclopediaCollection == null)
        {
            encyclopediaManager.GetComponent<EncyclopediaManager>().GetEncyclopediaCollection();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Flush();
            ToggleInventory();

            if (encyclopediaCollection == null)
            {
                Debug.LogError("encyclopediaCollection is NULL! Make sure it's initialized.");
                return;
            }

            if (encyclopediaCollection.encyclopedia == null)
            {
                Debug.LogError("encyclopediaCollection.encyclopedia is NULL! It should be initialized as a list.");
                return;
            }

            foreach (Encyclopedia encyclopedia in encyclopediaCollection.encyclopedia)
            {
                if (encyclopedia.caught)
                {
                    SC_Item foundItem = allFishItems.Find(item => item.itemID == encyclopedia.id);
                    if (foundItem != null)
                    {
                        this.AddItem(foundItem);
                    }
                    else
                    {
                        Debug.LogWarning($"Item with ID {encyclopedia.id} not found in allFishItems.");
                    }
                }
            }
        }
    }


    void ToggleInventory()
    {
        encyclopediaOpen = !encyclopediaOpen;
        inventoryUI.SetActive(encyclopediaOpen);

        Cursor.visible = encyclopediaOpen;
        Cursor.lockState = encyclopediaOpen ? CursorLockMode.None : CursorLockMode.Locked;

        if (encyclopediaOpen)
        {
            DisplayText();
        }
        else
        {
            Destroy(interactionText);
        }
    }

    public void AddItem(SC_Item newItem)
    {

        if (newItem == null)
        {
            Debug.LogError("Attempted to add a null item to inventory!");
            return;
        }

        items.Add(newItem);

        GameObject slot = Instantiate(inventorySlotPrefab, inventoryGrid); // Instantiates under UI
        Image itemIcon = slot.GetComponent<Image>();
        TMP_Text itemText = slot.GetComponentInChildren<TMP_Text>();

        if (itemIcon != null) itemIcon.sprite = newItem.itemIcon;
        if (itemText != null) itemText.text = newItem.itemName;
        encyclopediaManager.GetComponent<EncyclopediaManager>().SaveJson();
    }

    public List<SC_Item> getEncyclopediaItems()
    {
        return items;
    }

    public void Flush()
    {
        this.items.Clear();
        foreach (Transform child in inventoryGrid)
        {
            Destroy(child.gameObject);
        }
    }


    void DisplayText()
    {
        int c = 0;
        foreach (Encyclopedia encyclopedia in encyclopediaCollection.encyclopedia)
        {
            if (encyclopedia.caught)
            {
                c++;
            }
        }

        interactionText = new GameObject("DynamicText");
        interactionText.transform.SetParent(canvasTransform, false);

        TextMeshProUGUI newText = interactionText.AddComponent<TextMeshProUGUI>();
        newText.text = "Encyclopedia " + c + "/" + allFishItems.Count;
        newText.fontSize = 17;
        newText.color = Color.white;
        newText.alignment = TextAlignmentOptions.Center; // Align text to the right

        RectTransform rectTransform = newText.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(1, 1); // Top-right corner
        rectTransform.anchorMax = new Vector2(1, 1);
        rectTransform.pivot = new Vector2(1, 1); // Pivot to top-right
        rectTransform.anchoredPosition = new Vector2(-Screen.width / 2, -20); // Offset from top-right
    }
}
