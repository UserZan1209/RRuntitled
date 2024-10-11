using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonfires : MonoBehaviour
{
    [SerializeField] private SpriteRenderer fireSprite;
    [HideInInspector] private PlayerController pController;
    [SerializeField] private Vector2 myPos;
    void Awake()
    {
        fireSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        fireSprite.enabled = false;
        myPos = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            fireSprite.enabled = true;
            pController = collision.gameObject.GetComponent<PlayerController>();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            fireSprite.enabled = false; 
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
                pController.chr.RegenerateHealth();
                break;
        }
    }


    public Vector2 GetPosition()
    {
        return myPos;
    }
}
