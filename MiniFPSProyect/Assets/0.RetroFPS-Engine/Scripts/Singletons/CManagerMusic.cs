using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class CManagerMusic : MonoBehaviour
{
    // Start is called before the first frame update
    public static CManagerMusic Inst
    {
        get
        {
            if (_inst == null)
            {
                Debug.Log("Entra?");
                GameObject obj = new GameObject("Music");
                return obj.AddComponent<CManagerMusic>();
            }
            Debug.Log("Entra en el return");
            return _inst;

        }
    }
    private static CManagerMusic _inst;

  public void Awake()
    {
    if(_inst != null && _inst != this)
        {
            Destroy(gameObject);
            return;
        }
        //DontDestroyOnLoad(this.gameObject);
        _inst = this;
    }
    [SerializeField] public List<AudioClip> musicLists;

    [SerializeField] public AudioMixer audioMixer;

      [SerializeField] public bool IsAutoMusic= true;
    
   public void Start()
   {
    if(IsAutoMusic == true)
    {
        PlayMusicBackground(0);
    }

   }

    public void PlayMusic()
{
    if (musicLists.Count == 0) return;

    AudioSource audioSource = GetComponent<AudioSource>();
    if (audioSource == null)
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    audioSource.clip = musicLists[0];
    audioSource.Play();
}

  public void StopMusic()
{
    AudioSource audioSource = GetComponent<AudioSource>();
    if (audioSource!= null)
    {
        audioSource.Stop();
    }
}

  public void PauseMusic()
{
    AudioSource audioSource = GetComponent<AudioSource>();
    if (audioSource!= null)
    {
        audioSource.Pause();
    }
}

public void PlayMusicBackground(int id)
{
    // Buscar el AudioClip correspondiente al id
    AudioClip clip = musicLists[id];
    AudioSource soundObject = GetComponent<AudioSource>();
    soundObject.clip = clip;
    soundObject.Play();

}

   public void FadeIn(float duration = 1f)
{
    AudioSource audioSource = GetComponent<AudioSource>();
    if (audioSource!= null)
    {
        audioSource.volume = 0f;
        StartCoroutine(FadeInCoroutine(duration));
    }
}

private IEnumerator FadeInCoroutine(float duration)
{
    float timer = 0f;
    while (timer < duration)
    {
        timer += Time.deltaTime;
        float volume = Mathf.Lerp(0f, 1f, timer / duration);
        GetComponent<AudioSource>().volume = volume;
        yield return null;
    }
}
public void FadeOut(float duration = 1f)
{
    AudioSource audioSource = GetComponent<AudioSource>();
    if (audioSource!= null)
    {
        audioSource.volume = 1f;
        StartCoroutine(FadeOutCoroutine(duration));
    }
}

private IEnumerator FadeOutCoroutine(float duration)
{
    float timer = 0f;
    while (timer < duration)
    {
        timer += Time.deltaTime;
        float volume = Mathf.Lerp(1f, 0f, timer / duration);
        GetComponent<AudioSource>().volume = volume;
        yield return null;
    }
    GetComponent<AudioSource>().Stop();
}

}

