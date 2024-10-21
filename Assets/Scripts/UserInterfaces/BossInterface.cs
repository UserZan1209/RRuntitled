using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class BossInterface : MonoBehaviour
{
    [HideInInspector] public static BossInterface Instance;
    [HideInInspector] private Canvas bossCanvas;

    [SerializeField] private TextMeshProUGUI bossNameText;
    [SerializeField] private BossController bossController;
    [SerializeField] private Slider bossSlider;

    private void Awake()
    {
        Instance = this;
        bossCanvas = GetComponent<Canvas>();
        bossCanvas.enabled = false;
    }

    private void Update()
    {
        if(bossController != null)
        {
            bossSlider.value = bossController.chr.myStats.Health;

            if(bossController.chr.myStats.Health <= 0) { bossCanvas.enabled = false; }
        }
    }

    public void ActivateFight(string name, BossController controller)
    {
        bossCanvas.enabled = true;
        bossController = controller;
        bossNameText.text = name;
        bossSlider.maxValue = bossController.chr.myStats.mHealth;
    }
}
