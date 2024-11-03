using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCameraManager : MonoBehaviour
{
    // Start is called before the first frame update

     //Singleton
     public static CCameraManager Inst
    {
        get
        {
            if (_inst == null)
            {
                GameObject obj = new GameObject("CCameraManager");
                return obj.AddComponent<CCameraManager>();
            }
            return _inst;

        }
    }
    private static CCameraManager _inst;

    public Camera mainCamera;
    public Camera camera1;
    public Camera camera2;

public void Awake()
    {
    if(_inst != null && _inst != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
        _inst = this;
    }
 

public Camera GetMainCamera()
{
    return mainCamera;
}

public Camera GetCamera1()
{
    return camera1;
}

public Camera GetCamera2()
{
    return camera2;
}


}


