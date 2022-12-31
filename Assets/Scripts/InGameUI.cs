using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    // Shared variables
    [Header("Crab Amount")] 
    public int crabAmount;
    public TextMeshProUGUI crabAmountText;

    
    
    // Fever Time
    [Header("Fever Time Settings")]
    [SerializeField] private Slider slider;
    
    private float maxSliderValue;

    public bool isFeverTime = false;
    public GameObject feverTimeParticleSystem;

    // Instance for other classes to access
    public static InGameUI Instance;
    

    // Start is called before the first frame update
    void Start()
    {
        // Changing crab amount for other classes to access
        Instance.crabAmount = 10;
        
        slider.value = 0;
        maxSliderValue = PlayerManager.maxSliderValue;
        
        feverTimeParticleSystem.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        crabAmountText.text = "x " + Instance.crabAmount.ToString();
    }
    
    void LateUpdate()
    {

        // If the player is in fever time, the slider will fill down
        if(isFeverTime && slider.value > 0)
        {
            // 100 is the multiplier of the maxSliderValue (100) because the maximum of slider.value is 1.
            slider.value -= maxSliderValue * Time.deltaTime / (PlayerManager.feverTimeDuration * PlayerManager.feverTimeDurationCountMultiplier * 100);
        }
        
        if((slider.value * 100) >= maxSliderValue && !isFeverTime)
        {
            Instance.isFeverTime = true;
            // Trigger FeverTime
            feverTimeParticleSystem.SetActive(true);
        }
        
        if (((slider.value * 100) <= 0 && isFeverTime))
        {
            Instance.isFeverTime = false;
            // Stop FeverTime
            feverTimeParticleSystem.SetActive(false);
        }

    }
    
    public void SetSliderValue(float value)
    {
        slider.value += (value / PlayerManager.maxSliderValue) * PlayerManager.valueCountMultiplier;
    }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
}
