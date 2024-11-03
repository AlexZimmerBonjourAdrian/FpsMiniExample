using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSFX : MonoBehaviour
{
    [SerializeField] private AudioClip sound;

   [SerializeField]   private AudioSource audioSource;

    
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void PlaySFX()
    {
        audioSource.clip = sound;
        audioSource.Play();
    }

    public void StopSFX()
    {
        audioSource.Stop();
    }

    public void DestroySFX()
    {
        Destroy(gameObject);
    }

    public void SetLoopSound(bool loop)
    {
        audioSource.loop = loop;
    }


   

    public AudioClip GetAudioClip()
    {
        return this.sound;
    }
}
