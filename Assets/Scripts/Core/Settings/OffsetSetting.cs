using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetSetting : MonoBehaviour
{
    [SerializeField] private float minOffsetBeatTime = -0.25f;
    [SerializeField] private float maxOffsetBeatTime = 0.25f;
    [SerializeField] private TMPro.TMP_Text text;

    private void Start()
    {
        UpdateText();
    }

    public void OnOffsetValueIncrease()
    {
        BeatsManager.offsetBeatTime += 0.01f;
        BeatsManager.offsetBeatTime = Mathf.Clamp(BeatsManager.offsetBeatTime, minOffsetBeatTime, maxOffsetBeatTime);
        UpdateText();
    }

    public void OnOffsetValueDecrease()
    {
        BeatsManager.offsetBeatTime -= 0.01f;
        BeatsManager.offsetBeatTime = Mathf.Clamp(BeatsManager.offsetBeatTime, minOffsetBeatTime, maxOffsetBeatTime);
        UpdateText();
    }

    private void UpdateText()
    {
        text.text = BeatsManager.offsetBeatTime.ToString("f2");
    }
}
