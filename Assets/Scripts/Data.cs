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


[System.Serializable]
public struct MessageDataPlatformer
{
    public DataPlatformer[] data; 
}

[System.Serializable]
public struct DataPlatformer
{
    public string cardType; 
    public int x;
    public int y;
    public string type; 
}


[System.Serializable]
public struct ConversationalData
{
    public string _id;
    public string question;
    public string[] answers;
    public string cardType; 
}