using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class SC_Item : ScriptableObject
{
    public int itemID;
    public string itemName;
    public Sprite itemIcon;
}
