using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PointClickerEngine
{
[CreateAssetMenu(fileName = "NewAnimatioNData", menuName = "PointToClickEngine/Animatic")]
[Serializable]
public class CAnimaticData : ScriptableObject
{
   public int _id;
  

   public Sprite _Animatic;

   public CAnimaticData _NextAnimatic;
  [SerializeField] private bool _hasPlayed;
  [SerializeField] private bool _isReplay;

    public bool HasPlayed 
    { 
        get => _hasPlayed; 
        set => _hasPlayed = value; 
    }


//     public void PlayAnimatic(CAnimaticData animaticData)
// {
//     if (animaticData.HasPlayed)
//     {
//         // La animatic ya se ha reproducido, no hacer nada
//         return;
//     }

//     // Reproducir la animatic
//     // ...

//     // Marcar la animatic como reproducida
//     animaticData.HasPlayed = true;
// }
}
}