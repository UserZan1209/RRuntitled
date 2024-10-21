using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryController : MonoBehaviour
{
    [HideInInspector] public static InventoryController Instance;

    [SerializeField] private Canvas inventoryCanvas;
    [SerializeField] private int xStart;
    [SerializeField] private int yStart;
    [SerializeField] private int xSpacing;
    [SerializeField] private int ySpacing;
    [SerializeField] private int collumCount;

    Dictionary<InventorySlot, GameObject> itemsDisplayed = new Dictionary<InventorySlot, GameObject>();

    [SerializeField] private GameObject inventorySlotPrefab;
    [SerializeField] private Inventory playerInventory;

    [SerializeField] private GameObject[] slots; 

    private void Awake()
    {
        InitInventory();
    }

    public void InitInventory()
    {
        Instance = this;

        inventoryCanvas = GetComponent<Canvas>();
        if (inventoryCanvas.enabled) { inventoryCanvas.enabled = false; }

        CreateInventory();
    }

    private void CreateInventory()
    {
        ClearChilds();

        slots = new GameObject[playerInventory.Items.Count];

        for(int i = 0; i < playerInventory.Items.Count; i++)
        {
            var item = Instantiate(inventorySlotPrefab.gameObject, Vector3.zero, Quaternion.identity, transform);
            item.transform.localPosition = CalculatePosition(i);
            item.GetComponent<SlotData>().name.text = playerInventory.Items[i].itemData.name.ToString();
            item.GetComponent<SlotData>().amount.text = playerInventory.Items[i].amount.ToString();

            slots[i] = item;
        }
    }

    public void ToggleInventory()
    {
        inventoryCanvas.enabled = !inventoryCanvas.enabled;
        if (inventoryCanvas.enabled)
        {
            CreateInventory();
        }

    }

    private void ClearChilds()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name != "BGImage")
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
    }

    public Vector3 CalculatePosition(int slotNum)
    {
        return new Vector3(xStart + (xSpacing * (slotNum % collumCount)), yStart + (-ySpacing * (slotNum / collumCount )), 0.0f);
    }
}
