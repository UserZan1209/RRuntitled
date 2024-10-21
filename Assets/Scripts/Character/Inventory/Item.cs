using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public Sprite iconUI;
    public int indexNum;
    public string itemName;
    public string itemDescription;
    public ItemType itemType;


}
