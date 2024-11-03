using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public static class CGameEvents 
{   
    public static readonly CGameEvent<int> OnLevelComplete = new CGameEvent<int>(); 
    public static readonly CGameEvent<string> OnPlayerDeath = new CGameEvent<string>(); // Now holds a string
    public static readonly CGameEvent<string> OnDialogueTrigger = new CGameEvent<string>();
    public static readonly CGameEvent<string> OnDialogueEnd = new CGameEvent<string>();

}
