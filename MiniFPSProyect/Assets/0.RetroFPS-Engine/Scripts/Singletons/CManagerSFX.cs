using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Video;

public class CManagerSFX : MonoBehaviour
{   
    public static CManagerSFX Inst
    {
        get
        {
            if (_inst == null)
            {
                _inst = FindObjectOfType<CManagerSFX>();
                if (_inst == null)
                {
                    GameObject obj = new GameObject("ManagerSFX");
                    _inst = obj.AddComponent<CManagerSFX>();
                }
            }
            return _inst;
        }
    }

    private static CManagerSFX _inst;

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


    [SerializeField] public List<AudioClip> ListSFX;
    [SerializeField] public AudioMixer audioMixer;

    private List<GameObject> ListSounds;


    [SerializeField]public Dictionary<AudioClip, string> soundMap = new Dictionary<AudioClip, string>();
    public void Update()
    {
//    ListSounds.RemoveAll(sound => sound == null);
    }
    
    public void AddSound()
    {
    GameObject soundObject = new GameObject("Sound");
    soundObject.AddComponent<AudioSource>();
    ListSounds.Add(soundObject);
    }
  
 


public void PlaySound(int id)
{
    // Buscar el AudioClip correspondiente al id
    AudioClip clip = ListSFX[id];
    AudioSource soundObject = GetComponent<AudioSource>();
    soundObject.clip = clip;
    soundObject.Play();

}
public void StopSFX()
{
    foreach (GameObject sound in ListSounds)
    {
        sound.GetComponent<AudioSource>().Stop();
        
    }
}
    
    


}
