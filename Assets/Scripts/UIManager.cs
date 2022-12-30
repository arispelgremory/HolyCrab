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
        Debug.Log("Play!");
        levelToLoad = "Level1";
        PlayButtonSound();
        // fadeToBlackPanel.GetComponent<Animator>().SetTrigger("FadeToBlack");
        StartCoroutine(Wait());
    }

    public void OptionButtonClicked()
    {
        Debug.Log("Option");
        levelToLoad = "OptionMenuScene";
        PlayButtonSound();
        StartCoroutine(Wait());
    }

    public void QuitButtonClicked()
    {
        Debug.Log("Quit Game");
        PlayButtonSound();
        isQuit = true;
        StartCoroutine(Wait());
    }
}
