using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    
    [Header("Warning Settings")]
    [SerializeField] public int warningTime;
    [SerializeField] public TextMeshProUGUI warningText;
    [SerializeField] public GameObject warningPanel;
    public bool _isWarning = false;
    
    [Header("Game Over Canvas")]
    [SerializeField] public GameObject gameOverCanvas;
    private bool _isGameOver = false;
    
    [Header("Win Canvas")]
    [SerializeField] public GameObject winCanvas;
    private bool _isWin = false;
    
    [Header("Level Up Canvas")]
    [SerializeField] public GameObject levelUpCanvas;
    private bool _isLevelUp = false;

    [Header("Pause Canvas")]
    public bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    
    
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
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
        
        crabAmountText.text = "x " + Instance.crabAmount.ToString();
    }
    
    void LateUpdate()
    {
        UpdateFeverTime();
    }

    public bool NotAllowRenderOthers()
    {
        return _isWin || _isGameOver || _isLevelUp;
    }

    public void HasLost()
    {
        this._isGameOver = true;
        this.gameOverCanvas.SetActive(true);
        
    }
    

    public void HasWin()
    {
        this._isWin = true;
        // TODO: Add win canvas
    }

    public void LevelUp()
    {
        this._isLevelUp = true;
        this.levelUpCanvas.SetActive(true);
    }

    private void UpdateFeverTime()
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
    
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenuScene");
        Debug.Log("Loading Menu...");
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game!");
        Application.Quit();
    }
    
}
