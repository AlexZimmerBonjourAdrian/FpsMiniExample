using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PointClickerEngine
{
[CreateAssetMenu(fileName = "NewAnimatioNData", menuName = "PointToClickEngine/Animation")]
[Serializable]

public class CAnimData : ScriptableObject
{
    private string Name;
    public string description;
    //public new int Id;
    public List<List<Sprite>> AnimMachine;
}
}