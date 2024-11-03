using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Meele", menuName = "MonstersDemonsAndDragons/Meele")]
public class CWeaponMeeleData : ScriptableObject
{
    public new string name;
    [TextArea(10, 10)]
    public string descripcion;
    public float damage;
    public float rateAttack;
    public float distance;

}
