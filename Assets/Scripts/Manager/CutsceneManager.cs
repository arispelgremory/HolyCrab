using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour
{
    public GameObject csManager;

    public PlayableDirector playDirector;
    public static int currentCutscene = 0;
    public static bool isCompleted = false;
    
    // Start is called before the first frame update
    private void Start()
    {
        csManager = this.gameObject;
        playDirector = csManager.transform.GetChild(currentCutscene).GetComponent<PlayableDirector>();
        for (int i = 0; i < csManager.transform.childCount; i++)
        {
            csManager.transform.GetChild(i).gameObject.SetActive(false);
        }
        csManager.transform.GetChild(currentCutscene).gameObject.SetActive(true);
    }

    // Update is called once per frame
    private void Update()
    {
        if (playDirector.time >= playDirector.duration - 1.0f)
        {
            isCompleted = true;
        }

        if (isCompleted)
        {
            isCompleted = false;
            SceneManager.LoadScene(1);

            AudioListener[] aL = FindObjectsOfType<AudioListener>();
            for (int i = 0; i < aL.Length; i++)
            {
                //Destroy if AudioListener is not on the MainCamera
                if (!aL[i].CompareTag("MainCamera"))
                {
                    DestroyImmediate(aL[i]);
                }
            }
        }
    }
}
