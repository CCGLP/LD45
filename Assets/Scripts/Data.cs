using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct MessageData
{
    public int id;
    public string message; 
}


[System.Serializable]
public struct MessageStartGame
{
    public int gameType; 
   
}

[System.Serializable]
public struct MessageDecideGame
{
    public string cardName;
    public string gameType; 
}
