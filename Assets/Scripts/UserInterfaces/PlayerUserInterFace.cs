using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUserInterFace : MonoBehaviour
{
    public static PlayerUserInterFace instance;

    [Header("Bar Slider references")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider staminaSlider;
    [SerializeField] private Slider experienceSlider;

    [Header("Text references")]
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI expNeededText;
    
    void Start()
    {
        instance = this;
    }

    public void init(Character player)
    {
        healthSlider.maxValue = player.myStats.Health;
        staminaSlider.maxValue = player.myStats.Stamina;
        LevelUpUI(player);
    }

    public void UpdateHealthUI(float val)
    {
        healthSlider.value = val;
    }

    public void UpdateStaminaUI(float val)
    {
        staminaSlider.value = val;
    }

    public void LevelUpUI(Character player)
    {
        experienceSlider.maxValue = player.myStats.requiredExperiencePoints;
        levelText.text = player.myStats.Level.ToString();
        expNeededText.text = experienceSlider.maxValue.ToString();
        experienceSlider.value = 0;
    }

    public void UpdateRequiredExperience(float val)
    {
        expNeededText.text = val.ToString();
    } 

    public void UpdateEXPUI(float val)
    {
        experienceSlider.value = val;
    }

    public void UpdateLevelUI(float val)
    {
        levelText.text = val.ToString();
    }
}
