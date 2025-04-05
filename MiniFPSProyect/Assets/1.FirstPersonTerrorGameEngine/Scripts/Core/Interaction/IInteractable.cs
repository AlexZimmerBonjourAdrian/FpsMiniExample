using UnityEngine;

public interface IInteractable
{
    void OnInteractionStart();
    void OnInteractionComplete();
    void OnInteractionCancel();
    
    // Métodos opcionales para feedback
    void OnInteractionProgress(float progress);
    string GetInteractionPrompt();
    bool CanInteract();
} 