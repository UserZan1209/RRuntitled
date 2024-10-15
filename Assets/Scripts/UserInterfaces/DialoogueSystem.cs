using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialoogueSystem : MonoBehaviour
{
    [HideInInspector] public static DialoogueSystem instance;
    [HideInInspector] private Canvas thisCanvas;

    [SerializeField] private GameObject playerSide;
    [SerializeField] private GameObject NPCSide;

    [SerializeField] private TextMeshProUGUI playerText;
    [SerializeField] private TextMeshProUGUI NPCText;

    void Awake()
    {
        instance = this;
        thisCanvas = GetComponent<Canvas>();
        thisCanvas.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0) && thisCanvas.enabled == true) 
        {
            thisCanvas.enabled = false;
        }
    }

    public void OpenDialogueCanvas()
    {
        thisCanvas.enabled = true;  
    }

}
