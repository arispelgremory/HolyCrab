using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Codes.Scripts.SceneManagement
{
    public class AsyncLoader : MonoBehaviour
    {  
        [Header("Menu Screens")]
        [SerializeField] private GameObject loadingScreen;
        
        [Header("Slider")]
        [SerializeField] private Slider loadingSlider;
        
        public virtual void LoadLevel(string levelToLoad)
        {   
            loadingScreen.SetActive(true);
            
            // Run Async
            StartCoroutine(LoadLevelASync(levelToLoad));
        }

        IEnumerator LoadLevelASync(string levelToLoad)
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelToLoad);

            while (!loadOperation.isDone)
            {
                float progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);
                loadingSlider.value = progressValue;
                yield return null;
            }
        }
    }
}