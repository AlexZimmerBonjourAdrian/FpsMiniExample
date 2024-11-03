using  System.Collections;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;


public class CGameEvent<T> 
{
    private readonly List<Action<T>> listeners = new List<Action<T>>();

    public void Subscribe(Action<T> listener)
    {
        if (!listeners.Contains(listener))
            listeners.Add(listener);
    }

    public void Unsubscribe(Action<T> listener)
    {
        listeners.Remove(listener);
    }

    public void Publish(T eventData)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            try 
            {
                listeners[i]?.Invoke(eventData);
            } 
            catch (Exception ex)
            {
                Debug.LogError($"Error in event listener: {ex.Message}");
                // Handle the exception (e.g., log, disable the listener)
            }
        }
    }
}