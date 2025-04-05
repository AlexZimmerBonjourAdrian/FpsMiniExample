using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    [System.Serializable]
    public class AmbientSound
    {
        public string name;
        public AudioClip clip;
        public float volume = 1f;
        public float pitch = 1f;
        public bool loop = true;
        public float spatialBlend = 1f;
        public float minDistance = 1f;
        public float maxDistance = 20f;
        public float fadeInTime = 2f;
        public float fadeOutTime = 2f;
    }
    
    [Header("Configuración")]
    [SerializeField] private List<AmbientSound> ambientSounds = new List<AmbientSound>();
    [SerializeField] private float masterVolume = 1f;
    [SerializeField] private float tensionMultiplier = 1.5f;
    
    [Header("Referencias")]
    [SerializeField] private TensionManager tensionManager;
    [SerializeField] private TerrorPlayerController playerController;
    
    private Dictionary<string, AudioSource> activeSources = new Dictionary<string, AudioSource>();
    private float currentTension;
    
    private void Start()
    {
        if (tensionManager == null)
        {
            tensionManager = FindObjectOfType<TensionManager>();
        }
        
        if (playerController == null)
        {
            playerController = FindObjectOfType<TerrorPlayerController>();
        }
        
        // Suscribirse a eventos
        if (tensionManager != null)
        {
            tensionManager.OnTensionChanged += UpdateTension;
        }
        
        // Inicializar sonidos ambientales
        InitializeAmbientSounds();
    }
    
    private void InitializeAmbientSounds()
    {
        foreach (AmbientSound sound in ambientSounds)
        {
            if (sound.clip != null)
            {
                AudioSource source = gameObject.AddComponent<AudioSource>();
                source.clip = sound.clip;
                source.volume = sound.volume * masterVolume;
                source.pitch = sound.pitch;
                source.loop = sound.loop;
                source.spatialBlend = sound.spatialBlend;
                source.minDistance = sound.minDistance;
                source.maxDistance = sound.maxDistance;
                
                activeSources.Add(sound.name, source);
                
                if (sound.loop)
                {
                    StartCoroutine(FadeIn(source, sound.fadeInTime));
                }
            }
        }
    }
    
    private void UpdateTension(float tension)
    {
        currentTension = tension;
        UpdateSoundIntensities();
    }
    
    private void UpdateSoundIntensities()
    {
        float tensionRatio = currentTension / 100f;
        
        foreach (var source in activeSources.Values)
        {
            // Ajustar volumen basado en la tensión
            float baseVolume = source.volume / masterVolume;
            source.volume = baseVolume * masterVolume * (1f + (tensionRatio * tensionMultiplier));
            
            // Ajustar pitch basado en la tensión
            source.pitch = 1f + (tensionRatio * 0.2f);
        }
    }
    
    public void PlaySound(string soundName, bool fadeIn = true)
    {
        if (activeSources.TryGetValue(soundName, out AudioSource source))
        {
            if (fadeIn)
            {
                StartCoroutine(FadeIn(source, 1f));
            }
            else
            {
                source.Play();
            }
        }
    }
    
    public void StopSound(string soundName, bool fadeOut = true)
    {
        if (activeSources.TryGetValue(soundName, out AudioSource source))
        {
            if (fadeOut)
            {
                StartCoroutine(FadeOut(source, 1f));
            }
            else
            {
                source.Stop();
            }
        }
    }
    
    private System.Collections.IEnumerator FadeIn(AudioSource source, float duration)
    {
        float startVolume = 0f;
        float targetVolume = source.volume;
        
        source.volume = startVolume;
        source.Play();
        
        float elapsedTime = 0f;
        
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            source.volume = Mathf.Lerp(startVolume, targetVolume, elapsedTime / duration);
            yield return null;
        }
        
        source.volume = targetVolume;
    }
    
    private System.Collections.IEnumerator FadeOut(AudioSource source, float duration)
    {
        float startVolume = source.volume;
        float targetVolume = 0f;
        
        float elapsedTime = 0f;
        
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            source.volume = Mathf.Lerp(startVolume, targetVolume, elapsedTime / duration);
            yield return null;
        }
        
        source.Stop();
        source.volume = targetVolume;
    }
    
    public void SetMasterVolume(float volume)
    {
        masterVolume = volume;
        UpdateSoundIntensities();
    }
    
    private void OnDestroy()
    {
        if (tensionManager != null)
        {
            tensionManager.OnTensionChanged -= UpdateTension;
        }
        
        // Detener todos los sonidos
        foreach (var source in activeSources.Values)
        {
            source.Stop();
            Destroy(source);
        }
        
        activeSources.Clear();
    }
} 