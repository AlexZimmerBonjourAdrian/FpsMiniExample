using UnityEngine;

public class InteractableObject : MonoBehaviour, IInteractable
{
    [Header("Configuración")]
    [SerializeField] private string interactionPromptText = "Presiona E para interactuar";
    [SerializeField] private bool requiresHold = true;
    [SerializeField] private float interactionTime = 1f;
    
    [Header("Efectos")]
    [SerializeField] private float tensionAmount = 20f;
    [SerializeField] private bool playSound = true;
    [SerializeField] private AudioClip interactionSound;
    
    private TensionManager tensionManager;
    private AudioSource audioSource;
    private bool canInteract = true;
    
    private void Start()
    {
        tensionManager = FindObjectOfType<TensionManager>();
        
        if (playSound && interactionSound != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = interactionSound;
            audioSource.playOnAwake = false;
        }
    }
    
    public void OnInteractionStart()
    {
        if (!canInteract) return;
        
        if (playSound && audioSource != null)
        {
            audioSource.Play();
        }
        
        if (tensionManager != null)
        {
            tensionManager.AddTension(tensionAmount);
        }
    }
    
    public void OnInteractionComplete()
    {
        if (!canInteract) return;
        
        // Aquí puedes agregar la lógica específica de la interacción
        Debug.Log($"Interacción completada con {gameObject.name}");
        
        // Desactivar la interacción si es necesario
        canInteract = false;
    }
    
    public void OnInteractionCancel()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
    
    public void OnInteractionProgress(float progress)
    {
        // Actualizar efectos visuales durante la interacción
        transform.localScale = Vector3.one * (1f + (progress * 0.1f));
    }
    
    public string GetInteractionPrompt()
    {
        return interactionPromptText;
    }
    
    public bool CanInteract()
    {
        return canInteract;
    }
    
    // Método para reactivar la interacción
    public void ResetInteraction()
    {
        canInteract = true;
        transform.localScale = Vector3.one;
    }
} 