using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Default,
    Weapon,
    Armor,
    Consumable,
    KeyItem,
}

public class Item : ScriptableObject
{
    public GameObject itemPrefab;
    public int indexNum;
    public string itemName;
    public string itemDescription;
    public ItemType itemType;


}
