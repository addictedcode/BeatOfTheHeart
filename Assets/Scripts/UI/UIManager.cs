using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Slider enemyHP_UI;
    [SerializeField] private Slider playerHP_UI;
    [SerializeField] private Image playerIcon;

    [SerializeField] private Minotaur minotaurReference;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //enemyHP_UI.value = minotaurReference.GetHealth();
    }
}
