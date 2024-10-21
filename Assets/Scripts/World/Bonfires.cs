using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Bonfires : MonoBehaviour
{
    [SerializeField] private SpriteRenderer fireSprite;
    [HideInInspector] private PlayerController pController;
    [SerializeField] private Vector2 myPos;
    [SerializeField] private Light2D myLight;
    void Awake()
    {
        fireSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        fireSprite.enabled = false;
        myLight.enabled = false;
        myPos = transform.position;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            fireSprite.enabled = true;
            myLight.enabled = true;
            pController = other.gameObject.GetComponent<PlayerController>();
            pController.chr.RegenerateHealth();
            PlayerUserInterFace.instance.UpdateHealthUI(pController.chr.myStats.Health);
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            fireSprite.enabled = false;
            myLight.enabled = false;
        }
    }

    public Vector2 GetPosition()
    {
        return myPos;
    }
}
