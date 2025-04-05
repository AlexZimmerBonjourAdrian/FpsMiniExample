using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    [System.Serializable]
    public class EnemyType
    {
        public string enemyName;
        public GameObject enemyPrefab;
        public float health = 100f;
        public float damage = 20f;
        public float attackRange = 2f;
        public float detectionRange = 10f;
        public float patrolRadius = 5f;
        public float attackCooldown = 1f;
        public float screamRange = 15f;
        public float screamCooldown = 30f;
        public AudioClip[] screamSounds;
        public AudioClip[] attackSounds;
        public AudioClip[] deathSounds;
    }
    
    [Header("Configuración")]
    [SerializeField] private List<EnemyType> enemyTypes = new List<EnemyType>();
    [SerializeField] private int maxEnemies = 5;
    [SerializeField] private float spawnInterval = 30f;
    [SerializeField] private float difficultyIncreaseInterval = 300f; // 5 minutos
    
    [Header("Referencias")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private TensionManager tensionManager;
    [SerializeField] private AudioManager audioManager;
    
    private List<GameObject> activeEnemies = new List<GameObject>();
    private float difficultyMultiplier = 1f;
    private float lastSpawnTime;
    private float lastDifficultyIncrease;
    
    private void Start()
    {
        if (playerTransform == null)
        {
            playerTransform = FindObjectOfType<TerrorPlayerController>().transform;
        }
        
        if (tensionManager == null)
        {
            tensionManager = FindObjectOfType<TensionManager>();
        }
        
        if (audioManager == null)
        {
            audioManager = FindObjectOfType<AudioManager>();
        }
        
        // Iniciar sistema de spawn
        StartCoroutine(SpawnRoutine());
        StartCoroutine(DifficultyIncreaseRoutine());
    }
    
    private System.Collections.IEnumerator SpawnRoutine()
    {
        while (true)
        {
            if (activeEnemies.Count < maxEnemies)
            {
                SpawnEnemy();
            }
            
            yield return new WaitForSeconds(spawnInterval);
        }
    }
    
    private System.Collections.IEnumerator DifficultyIncreaseRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(difficultyIncreaseInterval);
            IncreaseDifficulty();
        }
    }
    
    private void SpawnEnemy()
    {
        if (enemyTypes.Count == 0) return;
        
        // Seleccionar tipo de enemigo aleatorio
        EnemyType enemyType = enemyTypes[Random.Range(0, enemyTypes.Count)];
        
        // Encontrar posición de spawn válida
        Vector3 spawnPosition = FindValidSpawnPosition();
        
        // Instanciar enemigo
        GameObject enemy = Instantiate(enemyType.enemyPrefab, spawnPosition, Quaternion.identity);
        
        // Configurar enemigo
        EnemyController controller = enemy.GetComponent<EnemyController>();
        if (controller != null)
        {
            controller.Initialize(enemyType, playerTransform, tensionManager, audioManager);
            controller.OnEnemyDeath += HandleEnemyDeath;
        }
        
        activeEnemies.Add(enemy);
    }
    
    public GameObject SpawnEnemy(Vector3 position)
    {
        if (enemyTypes.Count == 0 || activeEnemies.Count >= maxEnemies) return null;
        
        // Seleccionar tipo de enemigo aleatorio
        EnemyType enemyType = enemyTypes[Random.Range(0, enemyTypes.Count)];
        
        // Instanciar enemigo
        GameObject enemy = Instantiate(enemyType.enemyPrefab, position, Quaternion.identity);
        
        // Configurar enemigo
        EnemyController controller = enemy.GetComponent<EnemyController>();
        if (controller != null)
        {
            controller.Initialize(enemyType, playerTransform, tensionManager, audioManager);
            controller.OnEnemyDeath += HandleEnemyDeath;
        }
        
        activeEnemies.Add(enemy);
        return enemy;
    }
    
    private Vector3 FindValidSpawnPosition()
    {
        float maxAttempts = 10;
        float attemptRadius = 20f;
        
        for (int i = 0; i < maxAttempts; i++)
        {
            Vector2 randomCircle = Random.insideUnitCircle * attemptRadius;
            Vector3 spawnPos = playerTransform.position + new Vector3(randomCircle.x, 0f, randomCircle.y);
            
            // Verificar si la posición es válida
            if (Physics.Raycast(spawnPos + Vector3.up * 2f, Vector3.down, out RaycastHit hit, 4f))
            {
                NavMeshHit navHit;
                if (NavMesh.SamplePosition(hit.point, out navHit, 2f, NavMesh.AllAreas))
                {
                    return navHit.position;
                }
            }
        }
        
        // Si no se encuentra una posición válida, usar una posición por defecto
        return playerTransform.position + Vector3.forward * 10f;
    }
    
    private void HandleEnemyDeath(GameObject enemy)
    {
        activeEnemies.Remove(enemy);
        
        // Aumentar tensión al matar un enemigo
        if (tensionManager != null)
        {
            tensionManager.AddTension(15f);
        }
    }
    
    private void IncreaseDifficulty()
    {
        difficultyMultiplier += 0.2f;
        
        // Ajustar valores de los enemigos
        foreach (EnemyType enemyType in enemyTypes)
        {
            enemyType.health *= 1.1f;
            enemyType.damage *= 1.1f;
            enemyType.detectionRange *= 1.1f;
            enemyType.attackRange *= 1.1f;
        }
        
        // Aumentar número máximo de enemigos
        maxEnemies = Mathf.Min(maxEnemies + 1, 10);
        
        // Reducir intervalo de spawn
        spawnInterval = Mathf.Max(spawnInterval * 0.9f, 10f);
        
        Debug.Log($"Dificultad aumentada. Multiplicador: {difficultyMultiplier}");
    }
    
    public void KillAllEnemies()
    {
        foreach (GameObject enemy in activeEnemies.ToArray())
        {
            EnemyController controller = enemy.GetComponent<EnemyController>();
            if (controller != null)
            {
                controller.Die();
            }
        }
    }
    
    public void PauseEnemies(bool pause)
    {
        foreach (GameObject enemy in activeEnemies)
        {
            EnemyController controller = enemy.GetComponent<EnemyController>();
            if (controller != null)
            {
                controller.SetPaused(pause);
            }
        }
    }
    
    public int GetActiveEnemyCount()
    {
        return activeEnemies.Count;
    }
    
    public int GetMaxEnemies()
    {
        return maxEnemies;
    }
    
    public float GetDifficultyMultiplier()
    {
        return difficultyMultiplier;
    }
    
    private void OnDestroy()
    {
        KillAllEnemies();
    }
} 