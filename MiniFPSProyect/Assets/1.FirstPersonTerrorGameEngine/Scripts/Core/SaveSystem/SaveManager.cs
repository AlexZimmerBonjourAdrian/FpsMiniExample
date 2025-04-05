using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

public class SaveManager : MonoBehaviour
{
    [System.Serializable]
    public class GameSave
    {
        public float playerPositionX;
        public float playerPositionY;
        public float playerPositionZ;
        public float playerRotationY;
        public float currentTension;
        public float currentStamina;
        public List<string> triggeredEvents;
        public Dictionary<string, bool> collectedItems;
        public float playTime;
    }
    
    [Header("Configuración")]
    [SerializeField] private string saveFileName = "horror_game_save.dat";
    [SerializeField] private bool autoSave = true;
    [SerializeField] private float autoSaveInterval = 300f; // 5 minutos
    
    [Header("Referencias")]
    [SerializeField] private TerrorPlayerController playerController;
    [SerializeField] private TensionManager tensionManager;
    [SerializeField] private HorrorEventManager eventManager;
    
    private string savePath;
    private float lastSaveTime;
    
    private void Start()
    {
        savePath = Path.Combine(Application.persistentDataPath, saveFileName);
        
        if (autoSave)
        {
            StartCoroutine(AutoSaveRoutine());
        }
    }
    
    private System.Collections.IEnumerator AutoSaveRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(autoSaveInterval);
            SaveGame();
        }
    }
    
    public void SaveGame()
    {
        GameSave save = new GameSave();
        
        // Guardar posición y rotación del jugador
        if (playerController != null)
        {
            Vector3 position = playerController.transform.position;
            save.playerPositionX = position.x;
            save.playerPositionY = position.y;
            save.playerPositionZ = position.z;
            save.playerRotationY = playerController.transform.eulerAngles.y;
        }
        
        // Guardar estado del juego
        if (tensionManager != null)
        {
            save.currentTension = tensionManager.GetCurrentTension();
        }
        
        // Guardar eventos activados
        if (eventManager != null)
        {
         //  save.triggeredEvents = eventManager.GetTriggeredEvents();
        }
        
        // Guardar tiempo de juego
        save.playTime = Time.time;
        
        // Serializar y guardar
        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream stream = new FileStream(savePath, FileMode.Create))
        {
            formatter.Serialize(stream, save);
        }
        
        lastSaveTime = Time.time;
        Debug.Log("Juego guardado exitosamente");
    }
    
    public bool LoadGame()
    {
        if (!File.Exists(savePath))
        {
            Debug.LogWarning("No se encontró archivo de guardado");
            return false;
        }
        
        try
        {
            BinaryFormatter formatter = new BinaryFormatter();
            GameSave save;
            
            using (FileStream stream = new FileStream(savePath, FileMode.Open))
            {
                save = (GameSave)formatter.Deserialize(stream);
            }
            
            // Restaurar posición y rotación del jugador
            if (playerController != null)
            {
                Vector3 position = new Vector3(
                    save.playerPositionX,
                    save.playerPositionY,
                    save.playerPositionZ
                );
                playerController.transform.position = position;
                playerController.transform.rotation = Quaternion.Euler(0f, save.playerRotationY, 0f);
            }
            
            // Restaurar estado del juego
            if (tensionManager != null)
            {
            //    tensionManager.SetTension(save.currentTension);
            }
            
            // Restaurar eventos
            if (eventManager != null)
            {
              //  eventManager.SetTriggeredEvents(save.triggeredEvents);
            }
            
            Debug.Log("Juego cargado exitosamente");
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error al cargar el juego: {e.Message}");
            return false;
        }
    }
    
    public void DeleteSave()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            Debug.Log("Guardado eliminado");
        }
    }
    
    public bool HasSaveFile()
    {
        return File.Exists(savePath);
    }
    
    public float GetLastSaveTime()
    {
        return lastSaveTime;
    }
    
    private void OnApplicationQuit()
    {
        if (autoSave)
        {
            SaveGame();
        }
    }
} 