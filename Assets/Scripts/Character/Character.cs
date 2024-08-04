using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
    public GameObject Object;

    public float Health;

    public float baseAttack;
    public float moveSpeed;

    public bool isAlive;
    public bool canMove;

    public Character()
    {
        baseAttack = 10;
    }

    public void ChangeHealthValue(float h)
    {
        if ((Health += h) <= 0)
        {
            Health = 0;
            isAlive = false;
        }
        else
        {
            Health += h;
            Debug.Log("HP: " + Health);
        }
    }
}
