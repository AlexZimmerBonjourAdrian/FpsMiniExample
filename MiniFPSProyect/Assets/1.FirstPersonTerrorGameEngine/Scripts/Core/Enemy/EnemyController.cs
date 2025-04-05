using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    public delegate void EnemyDeathHandler(GameObject enemy);
    public event EnemyDeathHandler OnEnemyDeath;
    
    private EnemyManager.EnemyType enemyType;
    private Transform playerTransform;
    private TensionManager tensionManager;
    private AudioManager audioManager;
    private NavMeshAgent agent;
    private Animator animator;
    private AudioSource audioSource;
    
    private float currentHealth;
    private float lastAttackTime;
    private float lastScreamTime;
    private Vector3 lastKnownPlayerPosition;
    private bool isPaused;
    private bool isDead;
    
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }
    
    public void Initialize(EnemyManager.EnemyType type, Transform player, TensionManager tension, AudioManager audio)
    {
        enemyType = type;
        playerTransform = player;
        tensionManager = tension;
        audioManager = audio;
        
        currentHealth = enemyType.health;
        lastAttackTime = -enemyType.attackCooldown;
        lastScreamTime = -enemyType.screamCooldown;
        
        // Configurar NavMeshAgent
        agent.speed = 3.5f;
        agent.angularSpeed = 120f;
        agent.acceleration = 8f;
        agent.stoppingDistance = enemyType.attackRange;
        
        // Iniciar comportamiento
        StartCoroutine(BehaviorRoutine());
    }
    
    private System.Collections.IEnumerator BehaviorRoutine()
    {
        while (!isDead && !isPaused)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
            
            if (distanceToPlayer <= enemyType.detectionRange)
            {
                // Perseguir al jugador
                ChasePlayer();
                
                // Intentar atacar si está en rango
                if (distanceToPlayer <= enemyType.attackRange)
                {
                    AttackPlayer();
                }
                
                // Gritar ocasionalmente
                if (Time.time - lastScreamTime >= enemyType.screamCooldown)
                {
                    Scream();
                }
            }
            else
            {
                // Patrullar
                Patrol();
            }
            
            yield return new WaitForSeconds(0.1f);
        }
    }
    
    private void ChasePlayer()
    {
        lastKnownPlayerPosition = playerTransform.position;
        agent.SetDestination(lastKnownPlayerPosition);
        
        // Rotar hacia el jugador
        Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        
        // Actualizar animación
        if (animator != null)
        {
            animator.SetBool("IsChasing", true);
        }
    }
    
    private void Patrol()
    {
        if (!agent.hasPath || agent.remainingDistance < 0.1f)
        {
            // Generar nueva posición de patrulla
            Vector2 randomCircle = Random.insideUnitCircle * enemyType.patrolRadius;
            Vector3 randomPoint = transform.position + new Vector3(randomCircle.x, 0f, randomCircle.y);
            
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, enemyType.patrolRadius, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }
        }
        
        // Actualizar animación
        if (animator != null)
        {
            animator.SetBool("IsChasing", false);
        }
    }
    
    private void AttackPlayer()
    {
        if (Time.time - lastAttackTime >= enemyType.attackCooldown)
        {
            // Realizar ataque
            TerrorPlayerController player = playerTransform.GetComponent<TerrorPlayerController>();
            if (player != null)
            {
               // player.TakeDamage(enemyType.damage);
                
                // Reproducir sonido de ataque
                if (enemyType.attackSounds != null && enemyType.attackSounds.Length > 0)
                {
                    AudioClip attackSound = enemyType.attackSounds[Random.Range(0, enemyType.attackSounds.Length)];
                    audioSource.PlayOneShot(attackSound);
                }
                
                // Aumentar tensión
                if (tensionManager != null)
                {
                    tensionManager.AddTension(10f);
                }
            }
            
            lastAttackTime = Time.time;
            
            // Actualizar animación
            if (animator != null)
            {
                animator.SetTrigger("Attack");
            }
        }
    }
    
    private void Scream()
    {
        if (enemyType.screamSounds != null && enemyType.screamSounds.Length > 0)
        {
            AudioClip screamSound = enemyType.screamSounds[Random.Range(0, enemyType.screamSounds.Length)];
            audioSource.PlayOneShot(screamSound);
            
            // Aumentar tensión significativamente
            if (tensionManager != null)
            {
                tensionManager.AddTension(25f);
            }
        }
        
        lastScreamTime = Time.time;
        
        // Actualizar animación
        if (animator != null)
        {
            animator.SetTrigger("Scream");
        }
    }
    
    public void TakeDamage(float damage)
    {
        if (isDead) return;
        
        currentHealth -= damage;
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    public void Die()
    {
        if (isDead) return;
        
        isDead = true;
        agent.enabled = false;
        
        // Reproducir sonido de muerte
        if (enemyType.deathSounds != null && enemyType.deathSounds.Length > 0)
        {
            AudioClip deathSound = enemyType.deathSounds[Random.Range(0, enemyType.deathSounds.Length)];
            audioSource.PlayOneShot(deathSound);
        }
        
        // Actualizar animación
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }
        
        // Notificar muerte
        OnEnemyDeath?.Invoke(gameObject);
        
        // Destruir después de la animación
        Destroy(gameObject, 5f);
    }
    
    public void SetPaused(bool pause)
    {
        isPaused = pause;
        agent.enabled = !pause;
        
        if (animator != null)
        {
            animator.enabled = !pause;
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        // Visualizar rangos en el editor
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, enemyType.detectionRange);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemyType.attackRange);
        
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, enemyType.patrolRadius);
    }
} 