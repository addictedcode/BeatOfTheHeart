using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BorderUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image mainBorder;
    [SerializeField] private Image subBorder;
    
    [Header("Combo Settings")]
    [SerializeField] private PlayerComboSettingsSO comboSettings;
    [SerializeField] private bool matchColorWithComboMeter = true;
    [SerializeField] private bool useLevelOneColorOnMiss = false;
    
    private Animator animator;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        if (matchColorWithComboMeter)
        {
            GameManager.OnComboMeterUpdated += UpdateBorderColor;
            UpdateBorderColor();
        }
    }

    private void OnDisable()
    {
        if (matchColorWithComboMeter)
            GameManager.OnComboMeterUpdated -= UpdateBorderColor;
    }

    public void PlayBeatAnimation()
    {
        animator.SetTrigger("Beat");
    }

    private void UpdateBorderColor()
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
            mainBorder.color = comboSettings.MaxComboColor;
            subBorder.color = comboSettings.MaxComboColor;
        }
        else if (comboCount <= 0)
        {
            Color missColor = useLevelOneColorOnMiss
                ? comboSettings.Levels[comboLevel].MeterColor
                : comboSettings.MissComboColor;
            mainBorder.color = missColor;
            subBorder.color = missColor;
        }
        else
        {
            PlayerComboLevel levelSettings = comboSettings.Levels[comboLevel];
            Color levelMeterColor = levelSettings.MeterColor;

            mainBorder.color = levelMeterColor;
            subBorder.color = levelMeterColor;
        }
    }
}
