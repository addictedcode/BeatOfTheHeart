using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class ScreenShaker : MonoBehaviour
{
    // Start is called before the first frame update
    
    public void ShakeScreen(float delay)
    {
        GameManager.Instance.currentVC.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 1;
        StartCoroutine(Delay(delay));
        
        IEnumerator Delay(float time)
        {
            yield return new WaitForSeconds(time);
            GameManager.Instance.currentVC.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;

        }
    }
}
