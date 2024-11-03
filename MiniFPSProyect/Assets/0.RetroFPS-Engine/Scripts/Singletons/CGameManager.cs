using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UIElements;

public class CGameManager : MonoBehaviour
{
    // Referencias a otros managers
    public CManagerSFX sfxManager;

    // Estados del juego
   // [SerializeField] private int currentLevel;
    //[SerializeField] private int playerLives;

    public enum GameState
{
    MainMenu,
    Playing,
    Paused,
    GameOver,
    DosState,
    TresState,
    DialogueState,
    FPSState,
    InventorieState,
    VisualNovelState,
    OpenWorldState,
    PuzzleState,

};
 

    [SerializeField] private bool IsEndGame;

    
   
    [SerializeField] private bool PuzzleMode = false;

    [SerializeField] private float lerpDuration = 2f; // Duración de la transición
    private Coroutine cameraLerpCoroutine; // Para controlar la corrutina
   //private Transform originalCameraTransform; 
   private Vector3 originalCameraPosition; 
   private Quaternion originalCameraRotation;

    private GameState currentState;
   
    private Dictionary<GameState, CGameState> states; 
   
    //    states = new Dictionary<GameState, CGameState>()
    //     {
    //          { GameState.MainMenu, new CMainMenuState(this) },
    //          { GameState.Playing, new CPlayingState(this) },
    //          { GameState.Paused, new CPausedState(this) },
    //          { GameState.GameOver, new CGameOverState(this) },
    //         { GameState.DosState, new C2DState(this) },
    //          { GameState.TresState, new C3DState(this) },
    //           { GameState.DialogueState, new CDialogueState(this) },
    //             { GameState.FPSState, new CFPSState(this) },
    //               { GameState.InventorieState, new CInventorieState(this) },
    //                 { GameState.VisualNovelState, new CVisualNovelState(this) },
    //                   { GameState.OpenWorldState, new COpenWorldState(this) },
    //                     { GameState.PuzzleState, new CPuzzleState(this) },      
    //     }; 
    
    private GameObject PlayerGameobject;
     [SerializeField] private Transform originalCameraTransform;


    public void LearpCameraToPuzzle(Transform puzzleTransform)
    {
        
        if(PuzzleMode == false)
        { 
           
        // Detener la corrutina si ya hay una en progreso
            if (cameraLerpCoroutine != null )
            {
                StopCoroutine(cameraLerpCoroutine);
            }
            cameraLerpCoroutine = StartCoroutine(LerpCameraToPuzzle(puzzleTransform));
        }
        // Iniciar la corrutina de movimiento
       
    }

    // Singleton
     public static CGameManager Inst
    {
        get
        {
            if (_inst == null)
            {
                GameObject obj = new GameObject("GameManager");
                return obj.AddComponent<CGameManager>();
            }
            return _inst;

        }
    }
    private static CGameManager _inst;

    private AsyncOperation _CurrentLoadScene;
    
   public void Awake()
    {
    if(_inst != null && _inst != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
        _inst = this;
    }



    private void Start()
    // Métodos para inicializar y finalizar el juego 
    {
      
        PlayerGameobject = GameObject.FindGameObjectWithTag("Player");
         CMICILSPSystem.Instance.ApplyTemplate(CMICILSPSystem.Instance.Detective); 
        originalCameraPosition = Camera.main.transform.position;
        //   states = new Dictionary<GameState, CGameState>()
        // {
        //      { GameState.MainMenu, new CMainMenuState(this) },
        //      { GameState.Playing, new CPlayingState(this) },
        //      { GameState.Paused, new CPausedState(this) },
        //      { GameState.GameOver, new CGameOverState(this) },
        //     { GameState.DosState, new C2DState(this) },
        //      { GameState.TresState, new C3DState(this) },
        //       { GameState.DialogueState, new CDialogueState(this) },
        //         { GameState.FPSState, new CFPSState(this) },
        //           { GameState.InventorieState, new CInventorieState(this) },
        //             { GameState.VisualNovelState, new CVisualNovelState(this) },
        //               { GameState.OpenWorldState, new COpenWorldState(this) },
        //                 { GameState.PuzzleState, new CPuzzleState(this) },      
        // }; 
        originalCameraRotation = Camera.main.transform.rotation;
        
    }

    //     public void SwitchState(GameState newState)
    // {
    //     if (currentState != null)
    //     {
    //         currentState.Exit(); // Exit the current state
    //     }

    //     currentState = states[newState]; // Get the new state from the dictionary
    //     currentState.Enter(); // Enter the new state
    // }

    public void InitializeGame()
    { 
        // Cargar niveles, configurar controles, etc.

    }

    public void EndGame()
    {
        // Guardar puntuación, mostrar pantalla de game over, etc.
       
    }

    // Métodos para manejar eventos del juego
    public void OnPlayerDeath()
    {
        // Restar vida al jugador, mostrar efecto de muerte, etc.
    }

    public void OnLevelComplete()
    {
        // Incrementar nivel, mostrar pantalla de victoria, etc.
    }

    // Métodos para actualizar y renderizar el juego
    public void Update()
    {
        // Actualizar física, renderizar gráficos, etc. 
       
    }

    public void RenderGame()
    {
        // Renderizar gráficos, mostrar HUD, etc.
    } 


    public void SaveGame()
    {
        // Guardar puntuación, nivel actual, Data del inventario, Progreso, niveles completados
    }

    public void LoadGame()
    {
        // Cargar puntuación, nivel actual, Data del inventario, Progreso, niveles completados
    }

    public void CreateNewSaveGame()
    {
        // Crea una partida nueva en un slot, que gurda el progreso
    }

    public void AutoloadGamePostDeath()
    {
        // Carga el juego después de la muerte del jugador cuando se presiona cualquier tecla
    }

    public void DeleteInventarieItem()
    {
         // Elimina un item del inventario
    }

    public void SwitchMechanics2DTo3D(bool Genere)
    {
        // Cambia las mecanicas de 2D a 3D O 3D a 2d
    }


  public void LearpBackToOriginalPosition()
    {
        if (cameraLerpCoroutine != null )
        {
            StopCoroutine(cameraLerpCoroutine);
        }
        cameraLerpCoroutine = StartCoroutine(LerpCameraToBackOrigianlPosition());
    }
    private IEnumerator LerpCameraToPuzzle(Transform targetTransform)
    {

    float timeElapsed = 0f;
    Transform startingPos = PlayerGameobject.transform;
    Quaternion startingRot = Camera.main.transform.rotation;

    while (timeElapsed < lerpDuration)
    {
        Camera.main.transform.position = Vector3.Lerp(startingPos.position, targetTransform.position, timeElapsed / lerpDuration);
        Camera.main.transform.rotation = Quaternion.Lerp(startingPos.rotation, targetTransform.rotation, timeElapsed / lerpDuration);

        timeElapsed += Time.deltaTime;
        yield return null;
    }
    PuzzleMode = true;

    }

  private IEnumerator LerpCameraToBackOrigianlPosition()
    {
    float timeElapsed = 0f;
    Transform startingPos = Camera.main.transform;
    Quaternion startingRot = Camera.main.transform.rotation;

    while (timeElapsed < lerpDuration)
    {
        Camera.main.transform.position = Vector3.Lerp(startingPos.position,  originalCameraPosition, timeElapsed / lerpDuration);
        Camera.main.transform.rotation = Quaternion.Lerp(startingRot , originalCameraRotation, timeElapsed / lerpDuration);

        timeElapsed += Time.deltaTime;
        yield return null;
    }
    PuzzleMode = false;

    //Guardar la posición original si es la primera vez que se llama a LerpCamera
   }

    public bool GetPuzzleMode()
    {
        return PuzzleMode;
    }

       #region  DebugVariables

     
    // public void ActivateCamera1(Transform target)
    // {
    //     DeactivateAllCameras();
    //     CCameraManager.Inst.camera1.gameObject.SetActive(true);
    //     CCameraManager.Inst.camera1.transform.position = target.position;
    //     CCameraManager.Inst.camera1.transform.rotation = target.rotation;
    //     PuzzleMode = true;
    // }

    // public void ActivateCamera2(Transform target)
    // {
    //     DeactivateAllCameras();
    //     CCameraManager.Inst.camera2.gameObject.SetActive(true);
    //     CCameraManager.Inst.camera2.transform.position = target.position;
    //     CCameraManager.Inst.camera2.transform.rotation = target.rotation;
    //     PuzzleMode = true;
    // }

    // public void ActivateMainCamera()
    // {
    //     DeactivateAllCameras();
    //     CCameraManager.Inst.mainCamera.gameObject.SetActive(true);
    //     PuzzleMode = false;
    // }

    // private void DeactivateAllCameras()
    // {
    //     CCameraManager.Inst.mainCamera.gameObject.SetActive(false);
    //     CCameraManager.Inst.camera1.gameObject.SetActive(false);
    //     CCameraManager.Inst.camera2.gameObject.SetActive(false);
    // }
    #endregion
public abstract class CGameState  
{
  protected  CGameManager gameManager; // Variable para acceder al GameManager

        // Constructor que recibe el GameManager
        public CGameState(CGameManager gameManager) 
        {
            this.gameManager = gameManager;
        }

        // Métodos abstractos que deben ser implementados por las clases hijas
        public virtual void Enter()
        {

        }
        public virtual void Update()
         {

         }
        public virtual void Exit()
        {

        }
    }
}
