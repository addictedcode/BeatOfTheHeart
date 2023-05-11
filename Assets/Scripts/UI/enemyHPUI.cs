using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enemyHPUI : MonoBehaviour
{
    [SerializeField] private GameObject fillImage;
    // Start is called before the first frame update

    public void setHP(float HP)
    {
        fillImage.GetComponent<Image>().fillAmount = HP / 200f;
    }

    void Update()
    {
        if (GameManager.Instance != null)
            if(GameManager.Instance.Minotaur != null)
                 setHP(GameManager.Instance.Minotaur.Health);
    }
}
