using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class CLevelGeneric : MonoBehaviour
{
    protected virtual void CompleteRoom()
    {
        //Debug CompleteRoom
    }

    protected virtual void InitRoom()
    {
       //Init Room
    }

    protected virtual void DeathPlayer()
    {
        //Death Player
    }

    protected virtual void AutoLoadPostDeath()
    {
        //Load post death
    }

    public virtual bool GetIsComplete()
    {
        return false;
    }

}


