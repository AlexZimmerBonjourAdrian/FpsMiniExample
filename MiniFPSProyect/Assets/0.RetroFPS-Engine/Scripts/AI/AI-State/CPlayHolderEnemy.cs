using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPlayHolderEnemy : MonoBehaviour, IDamage
{
    public IEnemyState currentState; // Current state of the enemy
    public IdleState idleState = new IdleState(); // Instance of the IdleState
    public ChaseState chaseState = new ChaseState(); // Instance of the ChaseState

    public DeathState deathState = new DeathState();
    public HitState hitState = new HitState();
    public AttackPlayerMeleeState attackPlayerMeleeState = new AttackPlayerMeleeState();


    [SerializeField]public List<Sprite> StateAnimationList;
    private float Life;

    [SerializeField] public SpriteRenderer actualSpriteRenderer;

    void Start()
    {
        Life= 100;
        actualSpriteRenderer = GetComponent<SpriteRenderer>();
        actualSpriteRenderer.sprite = StateAnimationList[0];
        ChangeState(idleState); // Start in the Idle state

    }
    void Update()
    {
        if (currentState != null)
        {
            currentState.UpdateState(this); 
        }
    }
    public void ChangeState(IEnemyState newState)
    {
        if (currentState != null)
        {
            currentState.Exit(this); 
        }

        currentState = newState; 
        currentState.Enter(this); 
    }
    public bool ShouldChasePlayer()
    {
        var Player = GameObject.Find("Player");

        return Vector3.Distance(transform.position, Player.transform.position) < 10f;
    }
    public void MoveToPlayer()
    {
           var Player = GameObject.Find("Player");
           var speed = 10f;
        
        transform.position = Vector3.MoveTowards(transform.position, Player.transform.position, speed * Time.deltaTime);
    }
    public bool PlayerInRange()
    {
        var Player = GameObject.Find("Player");
        return Vector3.Distance(transform.position, Player.transform.position) < 1f;
    }
    public bool PlayerOutRange()
    {
         var Player = GameObject.Find("Player");
         return Vector3.Distance(transform.position, Player.transform.position) < 1f;
    }
    private void CheckLife()
    {
        if (Life <= 0)
        {
            ChangeState(deathState);
        }
        else 
        {
            if (!(currentState is HitState)) 
            {
                ChangeState(hitState);
            }
        }
    }
    public void OnDamage()
    {
        Life -= 20;
        Debug.Log("Life: " + Life);
        CheckLife();
    }  
}
public interface IEnemyState 
{
    void Enter(CPlayHolderEnemy enemy);
    void UpdateState(CPlayHolderEnemy enemy);
    void Exit(CPlayHolderEnemy enemy);
}
public class IdleState : IEnemyState
{
    public void Enter(CPlayHolderEnemy enemy)
    {
        Debug.Log("Enemy entered Idle state");
        // Logic when entering idle (e.g., play idle animation)
        enemy.actualSpriteRenderer.sprite = enemy.StateAnimationList[0];
        
    }

    public void UpdateState(CPlayHolderEnemy enemy)
    {
        // Idle behavior (e.g., look around, wait for a condition)
        // if (enemy.ShouldChasePlayer()) 
        // {
        //     enemy.ChangeState(enemy.chaseState); // Transition to ChaseState
        // }
    }
    public void Exit(CPlayHolderEnemy enemy)
    {
        Debug.Log("Enemy exited Idle state");
        // Logic when leaving idle
        //enemy.actualSpriteRenderer.sprite = enemy.StateAnimationList[0];
    }
}
public class ChaseState : IEnemyState
{
    public void Enter(CPlayHolderEnemy enemy)
    {
        Debug.Log("Enemy entered Chase state");
        enemy.actualSpriteRenderer.sprite = enemy.StateAnimationList[0];
    }

    public void UpdateState(CPlayHolderEnemy enemy)
    {
        // Chase behavior (e.g., move towards the player)
        enemy.MoveToPlayer();

        if (!enemy.ShouldChasePlayer())
        {
            enemy.ChangeState(enemy.idleState); // Transition back to IdleState
        }
    }
    
    public void Exit(CPlayHolderEnemy enemy)
    {
        Debug.Log("Enemy exited Chase state");
        // Logic when leaving chase
    }
}
public class AttackPlayerMeleeState : IEnemyState
{
    public void Enter(CPlayHolderEnemy enemy)
    {
        Debug.Log("Enemy entered Chase state");
        // Logic when entering chase (e.g., play chase animation)
    }

    public void UpdateState(CPlayHolderEnemy enemy)
    {
        // Chase behavior (e.g., move towards the player)
           // var Player = GameObject.Find("Player");
           
    }
    
    public void Exit(CPlayHolderEnemy enemy)
    {
        Debug.Log("Enemy exited Chase state");
        // Logic when leaving chase
    }
}
public class DeathState : IEnemyState
{
    public void Enter(CPlayHolderEnemy enemy)
    {
        Debug.Log("Enemy entered Death state");
        // Logic when entering death (e.g., play death animation, disable components)
        // Example:
        //enemy.gameObject.SetActive(false); // Or destroy the object
        enemy.actualSpriteRenderer.sprite = enemy.StateAnimationList[2];
    }

    public void UpdateState(CPlayHolderEnemy enemy)
    {
        // No behavior needed in death state, as the enemy is typically inactive
    }
    
    public void Exit(CPlayHolderEnemy enemy)
    {
        Debug.Log("Enemy exited Death state"); 
        // Logic when leaving death (unlikely to be needed)
    }
}

public class HitState : IEnemyState
{
    public void Enter(CPlayHolderEnemy enemy)
    {
       // Debug.Log("Enemy entered Hit state");
        // Logic when entering hit (e.g., play hit animation)
        enemy.actualSpriteRenderer.sprite = enemy.StateAnimationList[1];
    }

    public void UpdateState(CPlayHolderEnemy enemy)
    {
        // Hit behavior (e.g., short invincibility frames, transition back to idle/chase)
        // Example:
        // After a short delay, transition back to idle if not chasing, otherwise chase
        enemy.StartCoroutine(ReturnToPreviousState(enemy, .3f)); // 1 second delay
    }

    private IEnumerator ReturnToPreviousState(CPlayHolderEnemy enemy, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (enemy.ShouldChasePlayer())
        {
           // enemy.ChangeState(enemy.chaseState);
           enemy.ChangeState(enemy.idleState);
        }
        // else
        // {
            
        // }
    }
    
    public void Exit(CPlayHolderEnemy enemy)
    {
        Debug.Log("Enemy exited Hit state");
        // Logic when leaving hit
    }
}
