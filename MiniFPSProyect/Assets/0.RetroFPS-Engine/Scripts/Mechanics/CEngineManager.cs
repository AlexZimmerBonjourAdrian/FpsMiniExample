using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEngineManager : MonoBehaviour
{
     [SerializeField] protected bool IsDebug = true;
     public static CEngineManager Inst
    {
        get
        {
            if (_inst == null)
            {
                GameObject obj = new GameObject("EngineManager");

                return obj.AddComponent<CEngineManager>();
            }
            return _inst;
        }
    }
    private static  CEngineManager _inst;

    private void Awake()
    {
        if (_inst != null && _inst != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
        _inst = this;
    }
    public bool GetIsDebug()
    {
        return IsDebug;
    }
    void Update()
   
    {
        
        if(Input.GetKeyDown(KeyCode.S))
        {
            CMICILSPSystem.Instance.IncreaseStat(CMICILSPSystem.Stats.Sanity, 1);
            Debug.LogWarning(CMICILSPSystem.Instance.GetStat(CMICILSPSystem.Stats.Sanity));
        }
    }
}
