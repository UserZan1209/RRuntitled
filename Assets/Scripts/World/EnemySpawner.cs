using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyRef;
    [SerializeField] private GameObject[] Entites;
    [SerializeField] private int maxEntites = 1;
    [SerializeField] private int entityCounter;

    private void Awake()
    {
        entityCounter = 0;
        Entites = new GameObject[0];
    }
    void Update()
    {
        if(entityCounter < maxEntites)
        {
            Spawn(enemyRef);

        }
        
    }

    private void Spawn(GameObject g)
    {
        GameObject[] temp = Entites;
        Entites = new GameObject[temp.Length + 1];

        Entites[entityCounter] = Instantiate(g, transform.position, Quaternion.identity);
        entityCounter++;
    }
}
