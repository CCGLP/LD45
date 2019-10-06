using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 
public class Answer : MonoBehaviour
{
    
    public void Init(AnswerInfo info)
    {
        this.GetComponentInChildren<Button>().onClick.AddListener(()=> { info.onClick(); });
        this.GetComponentInChildren<TextMeshProUGUI>().text = info.name; 
    }

    void Update()
    {
        
    }
}


public enum GameType
{
    PLATFORM = 0,
    ACTION = 1,
    CONVERSATIONAL = 2,
    ASK = -1
}


public delegate void AnswerCall(); 
public struct AnswerInfo
{
    public AnswerCall onClick;
    public string name; 
}