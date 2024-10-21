using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Empty Inventory", menuName = "Inventory/Inventory Object")]
public class Inventory : ScriptableObject
{
    public List<InventorySlot> Items = new List<InventorySlot>();

    public void AddItem(Item data, int am)
    {
        bool hasItem = false;
        for(int i = 0; i < Items.Count; i++)
        {
            if(Items[i].itemData == data)
            {
                Items[i].IncreaseItemCount(am);
                hasItem = true;
                break;
            }
        }
        if (!hasItem)
        {
            Items.Add(new InventorySlot(data, am));
        }
    }

    public void ClearInventory()
    {
        Items = new List<InventorySlot>();
    }

}

[System.Serializable]
public class InventorySlot
{
    public Item itemData;
    public int amount;

    public InventorySlot(Item item, int am)
    {
        itemData = item;
        amount = am;
    }

    public void IncreaseItemCount(int itemCount)
    {
        amount += itemCount;
    }
}
