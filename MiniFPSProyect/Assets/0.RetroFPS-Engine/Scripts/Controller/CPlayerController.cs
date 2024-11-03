using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//New Controller Player


//Pattern State
public interface IPlayerState

{
    void Enter();
    void Exit();
    void Update();
}

public class CPlayerController : MonoBehaviour
{
  private IPlayerState currentState;

  private void ChargeState(IPlayerState newState)
  {
    currentState?.Exit();
    currentState = newState;
    currentState?.Enter();
  }

  public void Update()
  {
    currentState?.Update();
  }
}

//Pattern Strategy
public interface IMovementStrategy
{
    void Move(Transform playerTransform);
}

public class NormalMovement : IMovementStrategy
{
    public void  Move(Transform playerTransform)
    {
            //Logica de movimiento 
    }
}

public class HeavyMovement : IMovementStrategy
{
    public void Move(Transform playerTransform)
    {
            //Logica de movimiento pesado
    }
}


//Pattern Ovserver
public class PlayerEventManager : MonoBehaviour
{
    public delegate void PlayerStateChanged();
    public event PlayerStateChanged OnPlayerStateChanged;

    public void ChangeState()
    {
        OnPlayerStateChanged();
    }
    
}

//Componetn Pattern
public class MovementComponent : MonoBehaviour
{
    public void Move(Transform playerTransform)
    {
        //Logica de movimiento
    }
}

public class DashComponent: MonoBehaviour
{
    public void Dash()

    {
        //Logica de Dash
    }
}
