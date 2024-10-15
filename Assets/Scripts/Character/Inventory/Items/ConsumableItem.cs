using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Empty Consumable", menuName = "Inventory/New Consumable")]
public class ConsumableItem : Item
{
    public float recoverAmount;
    public float recoverTime;
}
