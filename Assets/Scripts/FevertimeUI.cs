using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FevertimeUI : MonoBehaviour
{
    
    public Slider slider;
    
    private float maxSliderValue;
    private float timeToMaxInSeconds;
    private float timeToMaxFill;
    
    
    // Start is called before the first frame update
    void Start()
    {
        timeToMaxInSeconds = PlayerManager.timeToMaxInSeconds;
        maxSliderValue = PlayerManager.maxSliderValue;
        timeToMaxFill = timeToMaxInSeconds / maxSliderValue;
    }
    
    void LateUpdate()
    {
        // Debug.Log(slider.value);
        if(slider.value < maxSliderValue)
        {
            slider.value += timeToMaxFill * Time.deltaTime;
        }
        else
        {
            slider.value = maxSliderValue / maxSliderValue;
        }
    }
}
