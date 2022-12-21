using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]

public class UIManager : MonoBehaviour
{
    private string levelToLoad;
    public AudioClip beep;
    private bool isQuit = false;

    public GameObject fadeToBlackPanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PlayButtonSound()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = beep;
        audio.Play();
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1.0f);
        if(isQuit)
        {
            Application.Quit();
        }
        else
        {
            SceneManager.LoadScene(levelToLoad);
        }
    }

    public void PlayButtonClicked()
    {
        levelToLoad = "HolyCrab";
        PlayButtonSound();
        fadeToBlackPanel.GetComponent<Animator>().SetTrigger("FadeToBlack");
        StartCoroutine(Wait());
    }

    public void OptionButtonClicked()
    {
        levelToLoad = "OptionMenuScene";
        PlayButtonSound();
        StartCoroutine(Wait());
    }

    public void QuitButtonClicked()
    {
        levelToLoad = "";
        PlayButtonSound();
        isQuit = true;
        StartCoroutine(Wait());
    }
}
