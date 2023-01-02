using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;    //  Create instance
    public AudioClip beep;  // Click sound
    private bool isQuit = false;    // Check the quit status

    [SerializeField] private GameObject _loaderCanvas;  // Place loading screen you want to load
    [SerializeField] private Image _progressBar;    // Progress bar you want ot sync with loading
    private float _target;

    void Awake() 
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //  Play clicked sound
    public void PlayButtonSound()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = beep;
        audio.Play();
    }

    public async void LoadScene(string sceneName)
    {
        _target = 0;
        _progressBar.fillAmount = 0;

        var scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;

        _loaderCanvas.SetActive(true);

        do
        {
            await Task.Delay(100);
            _target = scene.progress;
        } while (scene.progress < 0.9f);

        await Task.Delay(1000);

        scene.allowSceneActivation = true;
        _loaderCanvas.SetActive(false);
    }

    void Update()
    {
        _progressBar.fillAmount = Mathf.MoveTowards(_progressBar.fillAmount, _target, 3 * Time.deltaTime);
    }

    public void PrepareToStart()
    {
        SoundManager.Instance.PlayMusic("Preparation");
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game!");
        Application.Quit();
    }
}
