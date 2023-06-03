using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerHPUI : MonoBehaviour
{
    [SerializeField] private GameObject fillImage;
    [SerializeField] private GameObject bgImage;

    void Awake()
    {
        BeatsManager.OnBeat += startAnims;
    }

    private void OnDisable()
    {
        BeatsManager.OnBeat -= startAnims;
    }

    public void startAnims(float num)
    {
        fillImage.GetComponent<Animator>().enabled = true;
        bgImage.GetComponent<Animator>().enabled = true;
    }

    public void setHP(float HP)
    {
        fillImage.GetComponent<Image>().fillAmount = HP / 16f;
        bgImage.GetComponent<Image>().fillAmount = 1 - HP/16f;
    }

    void Update()
    {
        if(GameManager.Instance != null)
            setHP(GameManager.Instance.Player.Health);
    }
}
