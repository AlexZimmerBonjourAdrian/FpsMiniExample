using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CLevelManager : MonoBehaviour
{
  
    public static CLevelManager Inst
    {
        get
        {
            if (_inst == null)
            {
              
                GameObject obj = new GameObject("Level");
                return obj.AddComponent<CLevelManager>();
            }

            return _inst;

        }
    }
    private static CLevelManager _inst;

    private AsyncOperation _CurrentLoadScene;

  public void Awake()
    {
    if(_inst != null && _inst != this)
        {
            Destroy(gameObject);
            return;
        }
       // DontDestroyOnLoad(this.gameObject);
        _inst = this;
    }


    public bool IsLoadingScene()
    {
        return _CurrentLoadScene != null && !_CurrentLoadScene.isDone;
    }

    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void LoadScene(string name)

    {
        SceneManager.LoadScene(name);
    }

    public void LoadSceneAsync(string name)
    {
        _CurrentLoadScene = SceneManager.LoadSceneAsync(name);
    }

    public void LoadSceneAsyncAdditive(string name)
    {
        _CurrentLoadScene = SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
    }

     public int GetCurrentSceneID()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

   public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void ApplicationQuit()
    {
        Application.Quit();
    }

    /*

  public void LateUpdate()
  {
       if(_CurrentLoadScene.isDone)
      {
          _CurrentLoadScene = null;
      }
  }
*/
}
