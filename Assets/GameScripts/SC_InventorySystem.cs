using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SC_InventorySystem : MonoBehaviour
{
    public PlayerMovement playerController;
    public GameObject inventoryUI;  // UI Panel
    public Transform inventoryGrid; // Grid Layout Group
    public GameObject inventorySlotPrefab; // Slot Prefab

    private bool inventoryOpen = false;
    private List<SC_Item> inventoryItems = new List<SC_Item>();

    public List<SC_Item> allFishItems; // Assign all SC_Item assets in the Inspector

    private const int MAX_ITEM_COUNT = 49;


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
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventory();
        }
    }

    void ToggleInventory()
    {
        inventoryOpen = !inventoryOpen;
        inventoryUI.SetActive(inventoryOpen);

        Cursor.visible = inventoryOpen;
        Cursor.lockState = inventoryOpen ? CursorLockMode.None : CursorLockMode.Locked;

        playerController.canMove = !inventoryOpen; // Disable movement when inventory is open
    }

    public void AddItem(SC_Item newItem)
    {

        if (newItem == null)
        {
            Debug.LogError("Attempted to add a null item to inventory!");
            return;
        }

        inventoryItems.Add(newItem);

        GameObject slot = Instantiate(inventorySlotPrefab, inventoryGrid); // Instantiates under UI
        Image itemIcon = slot.GetComponent<Image>();
        TMP_Text itemText = slot.GetComponentInChildren<TMP_Text>();

        if (itemIcon != null) itemIcon.sprite = newItem.itemIcon;
        if (itemText != null) itemText.text = newItem.itemName;
    }

    public bool isFull()
    {
        return inventoryItems.Count >= MAX_ITEM_COUNT; 
    }

    public List<SC_Item> getInventoryItems()
    {
        return inventoryItems;
    }

    public void Flush()
    {
        this.inventoryItems.Clear();
        foreach (Transform child in inventoryGrid)
        {
            Destroy(child.gameObject);
        }
    }
}
