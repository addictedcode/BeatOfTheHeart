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

    public void setHP(int HP)
    {
        fillImage.GetComponent<Image>().fillAmount = .25f * HP;
        bgImage.GetComponent<Image>().fillAmount = 1f -  .25f * HP;
    }

    void Update()
    {
        if(GameManager.Instance != null)
            setHP(GameManager.Instance.Player.Health);
    }
}
