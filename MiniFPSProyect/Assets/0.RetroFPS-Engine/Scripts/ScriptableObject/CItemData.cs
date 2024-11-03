using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "MonstersDemonsAndDragons/Items")]
public class CItemData : ScriptableObject
{
    // Start is called before the first frame update
    public new string name;
    [TextArea (2,2)]
    public string description;
    /*
    public Sprite artwork;
    */
    public float healthRecover;
    public float delayRegen;
    public float regenerateTime;
    public int invulnerabilitytime;
    public int damageMultiply;

}
