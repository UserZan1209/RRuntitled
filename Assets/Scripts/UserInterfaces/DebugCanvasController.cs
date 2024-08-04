using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class DebugCanvasController : MonoBehaviour
{
    // A static instance allows use of the script from other points without a direct reference
    public static DebugCanvasController Instance;

    [Header("Canvas component references")]
    //container refers to the empty objects containing the relavent UI text objects
    [SerializeField] private GameObject playerContainer;
    [SerializeField] private GameObject targetContainer;
    [SerializeField] private GameObject systemContainer;

    [SerializeField] private bool isActive;

    #region player-variables
    [SerializeField] private TextMeshProUGUI playerHealthTMP;
    #endregion

    #region target-variables
    [SerializeField] private TextMeshProUGUI targetHealthTMP;
    #endregion

    #region system-details-variables
    [SerializeField] private TextMeshProUGUI FPSTMP;
    #endregion

    private void Awake()
    {
        Instance = this;

        if (!isActive)
        {
            playerContainer.SetActive(false);
            targetContainer.SetActive(false);
            systemContainer.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.F1))
        {
            //ints used to enable or disable independant menus
            ToggleUI(1, 1, 1);
        }

        if (systemContainer.activeInHierarchy)
        {
            FPSTMP.text = CalculateFPS();
        }
    }

    private void ToggleUI(int p, int t, int s)
    {
        if(p == 1) { playerContainer.SetActive(!playerContainer.activeInHierarchy); }
        if(p == 1) { targetContainer.SetActive(!playerContainer.activeInHierarchy); }
        if(s == 1) { systemContainer.SetActive(!systemContainer.activeInHierarchy); }
    }

    private string CalculateFPS()
    {
        string text = "";

        int fps = (int)(1f / Time.unscaledDeltaTime);
        text = fps.ToString();

        return text;
    }

    public void SetHealthOnUI(float h)
    {
        if(h <= 0) { SceneManager.LoadScene(0); }
        playerHealthTMP.text = h.ToString();
    }

    public void SetTargetHealthOnUI(string h)
    {
        playerHealthTMP.text = h;
    }
}
