using UnityEngine;
using System.Collections;

public class InteractionController : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private float interactionDistance = 2.5f;
    [SerializeField] private float holdTime = 1f;
    [SerializeField] private LayerMask interactableLayers;
    
    [Header("Feedback")]
    [SerializeField] private float handShakeAmount = 0.1f;
    [SerializeField] private float breathingIntensity = 0.05f;
    [SerializeField] private float interactionProgressSpeed = 1f;
    
    [Header("UI")]
    [SerializeField] private GameObject interactionPrompt;
    [SerializeField] private UnityEngine.UI.Image progressBar;
    
    private Camera playerCamera;
    private TerrorPlayerController playerController;
    private IInteractable currentInteractable;
    private float currentHoldTime;
    private bool isHolding;
    
    private void Start()
    {
        playerCamera = GetComponentInChildren<Camera>();
        playerController = GetComponent<TerrorPlayerController>();
        
        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);
    }
    
    private void Update()
    {
        HandleInteraction();
        UpdateInteractionUI();
    }
    
    private void HandleInteraction()
    {
        RaycastHit hit;
        bool hitSomething = Physics.Raycast(
            playerCamera.transform.position,
            playerCamera.transform.forward,
            out hit,
            interactionDistance,
            interactableLayers
        );
        
        if (hitSomething)
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            
            if (interactable != null)
            {
                if (currentInteractable != interactable)
                {
                    currentInteractable = interactable;
                    ShowInteractionPrompt();
                }
                
                if (Input.GetKey(KeyCode.E))
                {
                    if (!isHolding)
                    {
                        StartInteraction();
                    }
                    
                    UpdateHoldProgress();
                }
                else if (isHolding)
                {
                    CancelInteraction();
                }
            }
        }
        else if (currentInteractable != null)
        {
            HideInteractionPrompt();
            currentInteractable = null;
        }
    }
    
    private void StartInteraction()
    {
        isHolding = true;
        currentHoldTime = 0f;
        
        if (currentInteractable != null)
        {
            currentInteractable.OnInteractionStart();
        }
    }
    
    private void UpdateHoldProgress()
    {
        if (!isHolding) return;
        
        currentHoldTime += Time.deltaTime * interactionProgressSpeed;
        
        if (currentHoldTime >= holdTime)
        {
            CompleteInteraction();
        }
    }
    
    private void CompleteInteraction()
    {
        if (currentInteractable != null)
        {
            currentInteractable.OnInteractionComplete();
        }
        
        CancelInteraction();
    }
    
    private void CancelInteraction()
    {
        isHolding = false;
        currentHoldTime = 0f;
        
        if (currentInteractable != null)
        {
            currentInteractable.OnInteractionCancel();
        }
    }
    
    private void UpdateInteractionUI()
    {
        if (progressBar != null)
        {
            progressBar.fillAmount = currentHoldTime / holdTime;
        }
    }
    
    private void ShowInteractionPrompt()
    {
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(true);
        }
    }
    
    private void HideInteractionPrompt()
    {
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }
    }
    
    // Método para obtener el objeto interactivo actual
    public IInteractable GetCurrentInteractable()
    {
        return currentInteractable;
    }
    
    // Método para verificar si está interactuando
    public bool IsInteracting()
    {
        return isHolding;
    }
} 