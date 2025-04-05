using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class SpawnPoint
    {
        public Transform point;
        public float spawnRadius = 2f;
        public bool isActive = true;
        public float minDistanceToPlayer = 10f;
        public float maxDistanceToPlayer = 30f;
    }
    
    [Header("Configuración")]
    [SerializeField] private List<SpawnPoint> spawnPoints = new List<SpawnPoint>();
    [SerializeField] private float spawnCheckInterval = 5f;
    [SerializeField] private float tensionThreshold = 50f;
    [SerializeField] private float maxEnemiesPerSpawn = 2;
    [SerializeField] private float spawnCooldown = 60f;
    
    [Header("Referencias")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private TensionManager tensionManager;
    [SerializeField] private EnemyManager enemyManager;
    
    private float lastSpawnTime;
    private List<SpawnPoint> activeSpawnPoints = new List<SpawnPoint>();
    
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
        
        if (enemyManager == null)
        {
            enemyManager = FindObjectOfType<EnemyManager>();
        }
        
        // Inicializar puntos de spawn activos
        UpdateActiveSpawnPoints();
        
        // Iniciar sistema de spawn
        StartCoroutine(SpawnCheckRoutine());
    }
    
    private System.Collections.IEnumerator SpawnCheckRoutine()
    {
        while (true)
        {
            if (ShouldSpawnEnemies())
            {
                SpawnEnemies();
            }
            
            yield return new WaitForSeconds(spawnCheckInterval);
        }
    }
    
    private bool ShouldSpawnEnemies()
    {
        // Verificar cooldown
        if (Time.time - lastSpawnTime < spawnCooldown)
        {
            return false;
        }
        
        // Verificar tensión
        if (tensionManager != null && tensionManager.GetCurrentTension() < tensionThreshold)
        {
            return false;
        }
        
        // Verificar número máximo de enemigos
        if (enemyManager != null && enemyManager.GetActiveEnemyCount() >= enemyManager.GetMaxEnemies())
        {
            return false;
        }
        
        return true;
    }
    
    private void SpawnEnemies()
    {
        UpdateActiveSpawnPoints();
        
        if (activeSpawnPoints.Count == 0)
        {
            return;
        }
        
        int enemiesToSpawn = (int)Random.Range(1, maxEnemiesPerSpawn + 1);
        
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            if (enemyManager.GetActiveEnemyCount() >= enemyManager.GetMaxEnemies())
            {
                break;
            }
            
            SpawnPoint spawnPoint = GetRandomSpawnPoint();
            if (spawnPoint != null)
            {
                // Encontrar posición de spawn válida
                Vector3 spawnPosition = FindValidSpawnPosition(spawnPoint);
                
                // Instanciar enemigo
                GameObject enemy = enemyManager.SpawnEnemy(spawnPosition);
                if (enemy != null)
                {
                    // Aumentar tensión al spawnear un enemigo
                    if (tensionManager != null)
                    {
                        tensionManager.AddTension(20f);
                    }
                }
            }
        }
        
        lastSpawnTime = Time.time;
    }
    
    private void UpdateActiveSpawnPoints()
    {
        activeSpawnPoints.Clear();
        
        foreach (SpawnPoint point in spawnPoints)
        {
            if (!point.isActive) continue;
            
            float distanceToPlayer = Vector3.Distance(point.point.position, playerTransform.position);
            
            if (distanceToPlayer >= point.minDistanceToPlayer && 
                distanceToPlayer <= point.maxDistanceToPlayer)
            {
                activeSpawnPoints.Add(point);
            }
        }
    }
    
    private SpawnPoint GetRandomSpawnPoint()
    {
        if (activeSpawnPoints.Count == 0)
        {
            return null;
        }
        
        return activeSpawnPoints[Random.Range(0, activeSpawnPoints.Count)];
    }
    
    private Vector3 FindValidSpawnPosition(SpawnPoint spawnPoint)
    {
        float maxAttempts = 10;
        
        for (int i = 0; i < maxAttempts; i++)
        {
            Vector2 randomCircle = Random.insideUnitCircle * spawnPoint.spawnRadius;
            Vector3 spawnPos = spawnPoint.point.position + new Vector3(randomCircle.x, 0f, randomCircle.y);
            
            // Verificar si la posición es válida
            if (Physics.Raycast(spawnPos + Vector3.up * 2f, Vector3.down, out RaycastHit hit, 4f))
            {
                UnityEngine.AI.NavMeshHit navHit;
                if (UnityEngine.AI.NavMesh.SamplePosition(hit.point, out navHit, 2f, UnityEngine.AI.NavMesh.AllAreas))
                {
                    return navHit.position;
                }
            }
        }
        
        // Si no se encuentra una posición válida, usar el punto de spawn directamente
        return spawnPoint.point.position;
    }
    
    public void SetSpawnPointActive(int index, bool active)
    {
        if (index >= 0 && index < spawnPoints.Count)
        {
            spawnPoints[index].isActive = active;
            UpdateActiveSpawnPoints();
        }
    }
    
    public void SetAllSpawnPointsActive(bool active)
    {
        foreach (SpawnPoint point in spawnPoints)
        {
            point.isActive = active;
        }
        UpdateActiveSpawnPoints();
    }
    
    private void OnDrawGizmos()
    {
        // Visualizar puntos de spawn en el editor
        foreach (SpawnPoint point in spawnPoints)
        {
            if (point.point != null)
            {
                Gizmos.color = point.isActive ? Color.green : Color.red;
                Gizmos.DrawWireSphere(point.point.position, point.spawnRadius);
                
                // Dibujar rango de distancia al jugador
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(point.point.position, point.minDistanceToPlayer);
                Gizmos.DrawWireSphere(point.point.position, point.maxDistanceToPlayer);
            }
        }
    }
} 