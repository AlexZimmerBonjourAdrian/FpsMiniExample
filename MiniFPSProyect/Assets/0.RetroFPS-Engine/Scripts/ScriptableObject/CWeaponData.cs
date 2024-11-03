using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using System;
using UnityEditor;

[CreateAssetMenu(fileName = "New Weapon", menuName = "MonstersDemonsAndDragons/Weapon")]
public class CWeaponData : ScriptableObject
{
   
    public new string name;
    [TextArea(10, 10)]
    public string descripcion;
    public float Damage;
    public float fireRate;
    public float speedMovement;
    public float dispercion;
    public bool isRayCasting = false;
    //[DrawIf("SomeFloat")]
    public float distance;
    public float timeReload;
    
    
      

}
