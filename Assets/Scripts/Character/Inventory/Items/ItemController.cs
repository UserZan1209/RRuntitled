using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    [SerializeField] private Item itemData;
    [SerializeField] private int itemAmount;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().inventory.AddItem(itemData, itemAmount);
            Destroy(gameObject);
        }
    }

}
