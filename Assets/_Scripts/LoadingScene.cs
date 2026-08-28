using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    public GameObject loadingPanel;
    public Slider loadingSlider;
    
    //bool gameLoaded = true;
    //int currentLevel;
    //bool allow;
    //string levelName;
    
    // Start is called before the first frame update
    void Start()
    {
       
        StartCoroutine(loadGameScene());

    }
    AsyncOperation gameScene;
   
    IEnumerator loadGameScene()
    {
        gameScene = SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Additive);
       gameScene.allowSceneActivation = false;
        while (!gameScene.isDone)
        {
            float progress = Mathf.Clamp01(gameScene.progress/.9f) ;
            loadingSlider.value = progress;
            if (progress >= 1)
            {
                gameScene.allowSceneActivation = true;
                
            }
            yield return null;
        }
        SceneManager.UnloadSceneAsync("loadingScene");
    }
   
}
