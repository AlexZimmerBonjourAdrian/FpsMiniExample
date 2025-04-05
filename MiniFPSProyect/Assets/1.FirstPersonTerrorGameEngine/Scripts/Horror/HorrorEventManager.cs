using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HorrorEventManager : MonoBehaviour
{
    [System.Serializable]
    public class HorrorEvent
    {
        public string eventName;
        public float triggerChance = 0.5f;
        public float minTensionRequired = 30f;
        public float tensionAmount = 20f;
        public float cooldown = 60f;
        public bool isOneTime = false;
        public GameObject[] objectsToActivate;
        public AudioClip[] soundsToPlay;
        public float[] soundDelays;
        public float eventDuration = 5f;
    }
    
    [Header("Configuración")]
    [SerializeField] private List<HorrorEvent> horrorEvents = new List<HorrorEvent>();
    [SerializeField] private float eventCheckInterval = 5f;
    [SerializeField] private float minTimeBetweenEvents = 15f;
    
    [Header("Referencias")]
    [SerializeField] private TensionManager tensionManager;
    [SerializeField] private AudioManager audioManager;
    
    private Dictionary<string, float> lastEventTimes = new Dictionary<string, float>();
    private List<string> triggeredOneTimeEvents = new List<string>();
    
    private void Start()
    {
        if (tensionManager == null)
        {
            tensionManager = FindObjectOfType<TensionManager>();
        }
        
        if (audioManager == null)
        {
            audioManager = FindObjectOfType<AudioManager>();
        }
        
        StartCoroutine(EventCheckRoutine());
    }
    
    private IEnumerator EventCheckRoutine()
    {
        while (true)
        {
            CheckForEvents();
            yield return new WaitForSeconds(eventCheckInterval);
        }
    }
    
    private void CheckForEvents()
    {
        float currentTension = tensionManager.GetCurrentTension();
        float currentTime = Time.time;
        
        foreach (HorrorEvent horrorEvent in horrorEvents)
        {
            // Verificar si el evento ya fue activado (para eventos únicos)
            if (horrorEvent.isOneTime && triggeredOneTimeEvents.Contains(horrorEvent.eventName))
            {
                continue;
            }
            
            // Verificar cooldown
            if (lastEventTimes.ContainsKey(horrorEvent.eventName))
            {
                if (currentTime - lastEventTimes[horrorEvent.eventName] < horrorEvent.cooldown)
                {
                    continue;
                }
            }
            
            // Verificar tensión mínima requerida
            if (currentTension < horrorEvent.minTensionRequired)
            {
                continue;
            }
            
            // Verificar probabilidad de activación
            if (Random.value > horrorEvent.triggerChance)
            {
                continue;
            }
            
            // Verificar tiempo mínimo entre eventos
            if (lastEventTimes.Count > 0)
            {
                float lastEventTime = float.MaxValue;
                foreach (float time in lastEventTimes.Values)
                {
                    lastEventTime = Mathf.Min(lastEventTime, time);
                }
                
                if (currentTime - lastEventTime < minTimeBetweenEvents)
                {
                    continue;
                }
            }
            
            // Activar el evento
            StartCoroutine(TriggerHorrorEvent(horrorEvent));
        }
    }
    
    private IEnumerator TriggerHorrorEvent(HorrorEvent horrorEvent)
    {
        // Registrar el tiempo del evento
        lastEventTimes[horrorEvent.eventName] = Time.time;
        
        // Agregar a eventos únicos si corresponde
        if (horrorEvent.isOneTime)
        {
            triggeredOneTimeEvents.Add(horrorEvent.eventName);
        }
        
        // Activar objetos
        foreach (GameObject obj in horrorEvent.objectsToActivate)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }
        
        // Reproducir sonidos
        for (int i = 0; i < horrorEvent.soundsToPlay.Length; i++)
        {
            if (horrorEvent.soundsToPlay[i] != null)
            {
                yield return new WaitForSeconds(horrorEvent.soundDelays[i]);
                audioManager.PlaySound($"event_{horrorEvent.eventName}_{i}", false);
            }
        }
        
        // Agregar tensión
        tensionManager.AddTension(horrorEvent.tensionAmount);
        
        // Esperar la duración del evento
        yield return new WaitForSeconds(horrorEvent.eventDuration);
        
        // Desactivar objetos
        foreach (GameObject obj in horrorEvent.objectsToActivate)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
    }
    
    // Método para activar un evento específico manualmente
    public void TriggerEventByName(string eventName)
    {
        HorrorEvent horrorEvent = horrorEvents.Find(e => e.eventName == eventName);
        if (horrorEvent != null)
        {
            StartCoroutine(TriggerHorrorEvent(horrorEvent));
        }
    }
    
    // Método para reiniciar eventos únicos
    public void ResetOneTimeEvents()
    {
        triggeredOneTimeEvents.Clear();
    }
    
    // Método para agregar un nuevo evento dinámicamente
    public void AddHorrorEvent(HorrorEvent newEvent)
    {
        horrorEvents.Add(newEvent);
    }
    
    // Método para remover un evento
    public void RemoveHorrorEvent(string eventName)
    {
        horrorEvents.RemoveAll(e => e.eventName == eventName);
    }
} 