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
        timeToMaxInSeconds = 0;
        maxSliderValue = PlayerManager.maxSliderValue;
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
    
    void LateUpdate()
    {
        // Debug.Log(slider.value);
        
        // {
        //     slider.value += timeToMaxFill * Time.deltaTime;
        // }
        if(slider.value > maxSliderValue)
        {
            slider.value = maxSliderValue / maxSliderValue;
        }
    }
    
}
