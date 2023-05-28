using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ComboMeterUI : MonoBehaviour
{
    [SerializeField] private PlayerComboSettingsSO comboSettings;
    [SerializeField] private Image backgroundFill;
    [SerializeField] private Image radialFill;
    [SerializeField] private TMP_Text comboText;

    private void OnEnable()
    {
        backgroundFill.color = comboSettings.MissComboColor;
        
        GameManager.OnComboMeterUpdated += UpdateUI;
    }

    private void OnDisable()
    {
        GameManager.OnComboMeterUpdated -= UpdateUI;
    }

    public void UpdateUI()
    {
        int comboCount = 0;
        int comboLevel = 0;
        if (GameManager.Instance != null)
        {
            comboCount = GameManager.Instance.PlayerComboCount;
            comboLevel = GameManager.Instance.PlayerComboCurrentLevel;
        }

        if (comboSettings.CheckIfAtMaxLevel(comboLevel))
        {
            comboText.color = comboSettings.MaxComboColor;

            if (comboSettings.HasUniqueTextOnMax)
                comboText.text = comboSettings.UniqueMaxText;
            else
                comboText.text = comboCount + " X";

            radialFill.color = comboSettings.MaxComboColor;
            radialFill.fillAmount = 1;
        }
        else if (comboCount <= 0)
        {
            comboText.color = comboSettings.MissComboColor;
            
            if (comboSettings.HasUniqueTextOnMiss)
                comboText.text = comboSettings.UniqueMissText;
            else
                comboText.text = "0 X";

            radialFill.fillAmount = 0;
        }
        else
        {
            PlayerComboLevel levelSettings = comboSettings.Levels[comboLevel];
            Color levelMeterColor = levelSettings.MeterColor;
            int nextLevelThreshold = levelSettings.Threshold;
            int prevLevelThreshold = comboLevel > 0 ? comboSettings.Levels[comboLevel - 1].Threshold : 0;
            
            comboText.color = levelMeterColor;
            comboText.text = comboCount + " X";

            radialFill.color = levelMeterColor;
            float currentBeats = comboCount - prevLevelThreshold;
            float totalBeats = nextLevelThreshold - prevLevelThreshold;
            radialFill.fillAmount = currentBeats / totalBeats;
        }
    }
}
