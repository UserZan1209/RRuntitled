using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugCanvasController : MonoBehaviour
{
    // A static instance allows use of the script from other points without a direct reference
    public static DebugCanvasController Instance;

    [Header("Canvas component references")]
    //container refers to the empty objects containing the relavent UI text objects
    [SerializeField] private GameObject playerContainer;
    [SerializeField] private GameObject systemContainer;

    #region player-variables
    [SerializeField] private TextMeshProUGUI playerHealthTMP;
    #endregion

    #region system-details-variables
    [SerializeField] private TextMeshProUGUI FPSTMP;
    #endregion

    private void Awake()
    {
        Instance = this;

        playerContainer.SetActive(false);
        systemContainer.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.F1))
        {
            //ints used to enable or disable independant menus
            ToggleUI(1, 1);
        }

        if (systemContainer.activeInHierarchy)
        {
            FPSTMP.text = CalculateFPS();
        }
    }

    private void ToggleUI(int p, int s)
    {
        if(p == 1) { playerContainer.SetActive(!playerContainer.activeInHierarchy); }
        if(s == 1) { systemContainer.SetActive(!systemContainer.activeInHierarchy); }
    }

    private string CalculateFPS()
    {
        string text = "";

        int fps = (int)(1f / Time.unscaledDeltaTime);
        text = fps.ToString();

        return text;
    }
}
