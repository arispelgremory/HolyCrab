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

     public void PlayButtonSound()
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

    public void QuitButtonClicked()
    {
        Debug.Log("Quit Game");
        PlayButtonSound();
        isQuit = true;
        StartCoroutine(Wait());
    }
}
