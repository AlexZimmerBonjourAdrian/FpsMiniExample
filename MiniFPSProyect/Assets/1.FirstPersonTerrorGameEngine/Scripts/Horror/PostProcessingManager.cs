// using UnityEngine;
// using UnityEngine.Rendering.PostProcessing;

// public class PostProcessingManager : MonoBehaviour
// {
//     [Header("Referencias")]
//     [SerializeField] private PostProcessVolume postProcessVolume;
//     [SerializeField] private TerrorPlayerController playerController;
//     [SerializeField] private TensionManager tensionManager;
    
//     [Header("Efectos")]
//     [SerializeField] private float vignetteIntensity = 0.5f;
//     [SerializeField] private float chromaticAberrationIntensity = 0.5f;
//     [SerializeField] private float grainIntensity = 0.5f;
//     [SerializeField] private float lensDistortionIntensity = 0.5f;
    
//     private Vignette vignette;
//     private ChromaticAberration chromaticAberration;
//     private Grain grain;
//     private LensDistortion lensDistortion;
    
//     private void Start()
//     {
//         if (postProcessVolume == null)
//         {
//             postProcessVolume = GetComponent<PostProcessVolume>();
//         }
        
//         if (playerController == null)
//         {
//             playerController = FindObjectOfType<TerrorPlayerController>();
//         }
        
//         if (tensionManager == null)
//         {
//             tensionManager = FindObjectOfType<TensionManager>();
//         }
        
//         Obtener referencias a los efectos
//         postProcessVolume.profile.TryGetSettings(out vignette);
//         postProcessVolume.profile.TryGetSettings(out chromaticAberration);
//         postProcessVolume.profile.TryGetSettings(out grain);
//         postProcessVolume.profile.TryGetSettings(out lensDistortion);
        
//         Suscribirse a eventos
//         if (tensionManager != null)
//         {
//             tensionManager.OnTensionChanged += UpdatePostProcessing;
//         }
//     }
    
//     private void Update()
//     {
//         if (playerController != null)
//         {
//             UpdateBreathingEffect();
//         }
//     }
    
//     private void UpdatePostProcessing(float tension)
//     {
//         float tensionRatio = tension / 100f;
        
//         Actualizar efectos basados en la tensión
//         if (vignette != null)
//         {
//             vignette.intensity.value = tensionRatio * vignetteIntensity;
//         }
        
//         if (chromaticAberration != null)
//         {
//             chromaticAberration.intensity.value = tensionRatio * chromaticAberrationIntensity;
//         }
        
//         if (grain != null)
//         {
//             grain.intensity.value = tensionRatio * grainIntensity;
//         }
        
//         if (lensDistortion != null)
//         {
//             lensDistortion.intensity.value = tensionRatio * lensDistortionIntensity;
//         }
//     }
    
//     private void UpdateBreathingEffect()
//     {
//         if (lensDistortion != null)
//         {
//             Agregar efecto de respiración al efecto de distorsión
//             float breathingEffect = Mathf.Sin(Time.time * 2f) * 0.1f;
//             lensDistortion.intensity.value += breathingEffect;
//         }
//     }
    
//     Método para activar/desactivar efectos
//     public void SetEffectsEnabled(bool enabled)
//     {
//         if (vignette != null) vignette.enabled.value = enabled;
//         if (chromaticAberration != null) chromaticAberration.enabled.value = enabled;
//         if (grain != null) grain.enabled.value = enabled;
//         if (lensDistortion != null) lensDistortion.enabled.value = enabled;
//     }
    
//     Método para ajustar la intensidad de los efectos
//     public void SetEffectIntensities(float vignetteInt, float chromaticInt, float grainInt, float lensInt)
//     {
//         vignetteIntensity = vignetteInt;
//         chromaticAberrationIntensity = chromaticInt;
//         grainIntensity = grainInt;
//         lensDistortionIntensity = lensInt;
//     }
    
//     private void OnDestroy()
//     {
//         if (tensionManager != null)
//         {
//             tensionManager.OnTensionChanged -= UpdatePostProcessing;
//         }
//     }
// } 