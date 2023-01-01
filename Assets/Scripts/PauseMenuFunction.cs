using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuFunction : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    
    // Game UI stuffs
    protected InGameUI gameUI;

    private void Start()
    {
        gameUI = InGameUI.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        // If either win or lose, then return
        if (gameUI.NotAllowRenderOthers()) return;
        
        
    }

    
}
