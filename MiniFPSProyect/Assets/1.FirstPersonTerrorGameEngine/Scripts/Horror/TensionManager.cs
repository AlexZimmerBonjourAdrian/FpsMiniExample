using UnityEngine;
using System.Collections;

public class TensionManager : MonoBehaviour
{
    [Header("Configuración de Tensión")]
    [SerializeField] private float maxTension = 100f;
    [SerializeField] private float tensionDecayRate = 5f;
    [SerializeField] private float tensionBuildRate = 10f;
    
    [Header("Efectos de Tensión")]
    [SerializeField] private float maxHeartbeatRate = 1.5f;
    [SerializeField] private float maxBreathingRate = 2f;
    [SerializeField] private float maxCameraShake = 0.1f;
    
    [Header("Eventos")]
    [SerializeField] private float tensionThresholdForEvents = 70f;
    [SerializeField] private float eventCooldown = 30f;
    
    private float currentTension;
    private float lastEventTime;
    private TerrorPlayerController playerController;
    private AudioSource heartbeatSource;
    private AudioSource breathingSource;
    
    // Eventos del sistema
    public delegate void TensionEvent(float tension);
    public event TensionEvent OnTensionChanged;
    public event TensionEvent OnTensionThresholdReached;
    
    private void Start()
    {
        playerController = FindObjectOfType<TerrorPlayerController>();
        SetupAudioSources();
        StartCoroutine(TensionDecayRoutine());
    }
    
    private void SetupAudioSources()
    {
        // Configurar fuente de latidos
        heartbeatSource = gameObject.AddComponent<AudioSource>();
        heartbeatSource.loop = true;
        heartbeatSource.playOnAwake = false;
        
        // Configurar fuente de respiración
        breathingSource = gameObject.AddComponent<AudioSource>();
        breathingSource.loop = true;
        breathingSource.playOnAwake = false;
    }
    
    public void AddTension(float amount)
    {
        currentTension = Mathf.Min(maxTension, currentTension + amount);
        UpdateTensionEffects();
        
        if (currentTension >= tensionThresholdForEvents && Time.time - lastEventTime >= eventCooldown)
        {
            TriggerTensionEvent();
        }
    }
    
    private void UpdateTensionEffects()
    {
        float tensionRatio = currentTension / maxTension;
        
        // Actualizar efectos de audio
        if (heartbeatSource != null)
        {
            heartbeatSource.pitch = 1f + (tensionRatio * (maxHeartbeatRate - 1f));
        }
        
        if (breathingSource != null)
        {
            breathingSource.pitch = 1f + (tensionRatio * (maxBreathingRate - 1f));
        }
        
        // Actualizar efectos visuales
        if (playerController != null)
        {
         //   playerController.AddFear(tensionRatio * 100f);
        }
        
        // Disparar evento de cambio de tensión
        OnTensionChanged?.Invoke(currentTension);
    }
    
    private void TriggerTensionEvent()
    {
        lastEventTime = Time.time;
        OnTensionThresholdReached?.Invoke(currentTension);
    }
    
    private IEnumerator TensionDecayRoutine()
    {
        while (true)
        {
            if (currentTension > 0)
            {
                currentTension = Mathf.Max(0f, currentTension - tensionDecayRate * Time.deltaTime);
                UpdateTensionEffects();
            }
            yield return null;
        }
    }
    
    // Métodos públicos para consulta
    public float GetCurrentTension()
    {
        return currentTension;
    }
    
    public float GetTensionRatio()
    {
        return currentTension / maxTension;
    }
    
    // Método para configurar eventos de tensión
    public void SetTensionEvent(float threshold, float cooldown)
    {
        tensionThresholdForEvents = threshold;
        eventCooldown = cooldown;
    }
} 