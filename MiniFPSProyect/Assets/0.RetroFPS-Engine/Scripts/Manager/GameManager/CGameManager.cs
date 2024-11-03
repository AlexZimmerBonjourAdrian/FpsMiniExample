using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Plataform2D
{
public class CGameManager : MonoBehaviour
{
   // public CPlayerManager[] _player;
    [SerializeField]public Transform _transformSpawnWeapond;
    public static CGameManager Inst
    {
        get
        {
            if (_inst == null)
            {
                GameObject obj = new GameObject("CGameManager");
                return obj.AddComponent<CGameManager>();
            }
            return _inst;
        }
    }
    private static CGameManager _inst;
    private AsyncOperation _currentLoadScene;

    #region ValueManagerPlayer
  
    private GameObject _ActualPlayer;
    public GameObject _PlayerNatalia;
    public GameObject _PlayerLoop;
    
    #endregion

    public void Awake()
    {
        if (_inst != null && _inst != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
        _inst = this;
    }
    

    public void Update()
    {

        //Debug.Log(IsLoadingScene());
       /* if(Input.GetKeyDown(KeyCode.Tab))
        {
            spawnPlayer();
        }*/
        // if(Input.GetKeyDown(KeyCode.Tab))
        // {
        //     CManagerWeapond.Inst.SpawnWeapond(_transformSpawnWeapond.position);
        // }

    }
    /*
    private void spawnPlayer()
    {
        
        
        for (int i = 0; i < _player.Length;i++)
        {
            if (_player[i]._PlayerNumber == 0)// _PlayerNumber == 0)
            {
                _ActualPlayer = _PlayerLoop;
            }
            else if (_player[i]._PlayerNumber== 1)
            {
                _ActualPlayer = _PlayerNatalia;
            }

            _player[i]._Instance = Instantiate(_ActualPlayer, _player[i]._SpawnPoint.position, _player[i]._SpawnPoint.rotation) as GameObject;
                //_player[i]._PlayerNumber = i + 1;
                _player[i].Setup();
         }
         
    }
    */
    #region ControlDeEcenarios
    public void LateUpdate()
    {
        if(_currentLoadScene != null)
        {
            if (_currentLoadScene.isDone)
                _currentLoadScene = null;
        }
    }
    public bool IsLoadingScene()
    {
        return _currentLoadScene != null && !_currentLoadScene.isDone;
    // Start is called before the first frame updat
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
        _currentLoadScene = SceneManager.LoadSceneAsync(name);
    }
    public void LoadSceneAsyncAdditive(string name)
    {
        _currentLoadScene = SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
    }
    #endregion


}
} 
