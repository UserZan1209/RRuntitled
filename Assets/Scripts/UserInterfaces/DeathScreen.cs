using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathScreen : MonoBehaviour
{
    [HideInInspector] public static DeathScreen Instance;
    [SerializeField] private Canvas deathCanvas;

    [HideInInspector] private GameObject player;
    void Awake()
    {
        Instance = this;
        deathCanvas = GetComponent<Canvas>();
        deathCanvas.enabled = false;
    }

    void Update()
    {
        if (deathCanvas.enabled == true)
        {
            if (Input.GetKeyUp(KeyCode.R))
            {
                player.GetComponent<PlayerController>().Respawn();
                Time.timeScale = 1.0f;
                deathCanvas.enabled = false;
            }
        }
    }

    public void OpenDeathMenu(GameObject p)
    {
        player = p;
        Time.timeScale = 0.3f;
        deathCanvas.enabled = true;
    }
}
