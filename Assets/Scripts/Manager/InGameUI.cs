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
    
    [Header("Objective")]
    [SerializeField] public int requirementCount;
    [SerializeField] public TextMeshProUGUI requirementText;
    [SerializeField] public GameObject objectivePanel;
    private int objectiveTimeset = 0;
    private float objectiveTimer = 0.0f;

    [Header("Warning Settings")]
    [SerializeField] public int warningTime;
    [SerializeField] public TextMeshProUGUI warningText;
    [SerializeField] public GameObject warningPanel;
    public bool _isWarning = false;
    
    [Header("Game Over Canvas")]
    [SerializeField] public GameObject gameOverCanvas;
    private bool _isGameOver = false;
    
    [Header("Win Canvas")]
    [SerializeField] private GameObject winCanvas;
    [SerializeField] private TextMeshProUGUI crabAmountWhenWin;
    [SerializeField] private TextMeshProUGUI defeatedEnemyAmountWhenWin;
    [SerializeField] public int enemyDeafeated = 0;
    private bool _isWin = false;
    
    //  TODO: DLC
    // [Header("Level Up Canvas")]
    // [SerializeField] public GameObject levelUpCanvas;
    // private bool _isLevelUp = false;

    [Header("Pause Canvas")]
    public bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    
    [Header("Timer Canvas")]
    public int timeCount;
    private float _timer;
    private int _minutes;
    private int _seconds;
    [SerializeField] private TextMeshProUGUI _timerText;

    // Instance for other classes to access
    public static InGameUI Instance;
    

    // Start is called before the first frame update
    void Start()
    {
        // Changing crab amount for other classes to access
        // Instance.crabAmount = 10;
        Time.timeScale = 1;
        slider.value = 0;
        maxSliderValue = PlayerManager.maxSliderValue;
        
        feverTimeParticleSystem.SetActive(false);

        _minutes = timeCount / 60;
        _seconds = timeCount % 60;
        
        // Initialize the objective to the textmesh pro
        
        requirementText.text = requirementCount.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        // If either win or lose, then return
        if (NotAllowRenderOthers()) return;   
        
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
    
        _timer += Time.deltaTime;
        if(_timer >= 1.0f)
        {
            timeCount--;
            _timer = 0;
        }

        _minutes = timeCount / 60;
        _seconds = timeCount % 60;
        
        if(timeCount <= 0)
        {
            timeCount = 0;
            _minutes = 0;
            _seconds = 0;
        }
        

        string outputSeconds = (timeCount % 60) < 10 ? "0" + _seconds : _seconds.ToString();
        _timerText.text = "0" + _minutes + ":" + outputSeconds;
    
        // Show the Objective for five seconds
        objectiveTimer += Time.deltaTime;
        if(objectiveTimer > 5.0f)
        {
            objectivePanel.SetActive(false);
        }
        else
        {
            objectivePanel.SetActive(true);
        }
    }
    
    void LateUpdate()
    {
        UpdateFeverTime();
    }

    public bool NotAllowRenderOthers()
    {
        return _isWin || _isGameOver || GameIsPaused;
    }

    public void HasLost()
    {
        this._isGameOver = true;
        this.gameOverCanvas.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
        SoundManager.Instance.musicSource.volume = 0.2f;
    }
    

    public void HasWin()
    {
        this._isWin = true;
        this.crabAmountWhenWin.text = "" + crabAmount;
        this.defeatedEnemyAmountWhenWin.text = "" + enemyDeafeated;
        this.winCanvas.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
    }

    // TODO: DLC
    // public void LevelUp()
    // {
    //     this._isLevelUp = true;
    //     this.levelUpCanvas.SetActive(true);
    // }

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
            SoundManager.Instance.PlayMusic("FeverTime");
        }
        
        if (((slider.value * 100) <= 0 && isFeverTime))
        {
            Instance.isFeverTime = false;
            // Stop FeverTime
            feverTimeParticleSystem.SetActive(false);
            SoundManager.Instance.PlayMusic("Preparation");
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
        SoundManager.Instance.musicSource.volume = 0.5f;

    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 0f;
        GameIsPaused = true;
        SoundManager.Instance.musicSource.volume = 0.2f;
    }

    public void ResetGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("Reset!");
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game!");
        Application.Quit();
    }
    
}
