using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ConversationalQuestionPhase
{
    QUESTION,
    ASK1,
    ASK2,
    ASK3,
    ASK4
}
public class ConversationalGameController : Singleton<ConversationalGameController>
{
    bool canAddAnswers = true;

    ConversationalData nextDataToSend;
    ConversationalQuestionPhase phase; 
    void Start()
    {
        //Le pido al server que por favor me de algo
        NetworkController.Instance.SendWebSocketMessage(6, NetworkController.Instance.GameType); 
    }

    public void InitializeQuestionAnswer(ConversationalData data)
    {
        AnswerQuestionPanel.Instance.ClearAnswers();

        AnswerQuestionPanel.Instance.ChangeQuestion(data.question);
        nextDataToSend = new ConversationalData();
        nextDataToSend.cardType = NetworkController.Instance.GameType; 
        var answers = new List<AnswerInfo>();
        for (int i = 0; i < data.answers.Length; i++)
        {
            string possibleNewId = data.answers[i] + data.question;
            ConversationalData newData = new ConversationalData { cardType = NetworkController.Instance.GameType, question = "whateve", _id = possibleNewId };
            answers.Add(new AnswerInfo
            {
                name = data.answers[i],
                onClick = () =>
                {
                    this.nextDataToSend._id = newData._id;
                    NetworkController.Instance.SendWebSocketMessage(8, JsonUtility.ToJson(newData));
                }
            }); 
        }

        AnswerQuestionPanel.Instance.ShowAnswers(answers); 
    }

    public void AskForNewQuestionInit()
    {
        AnswerQuestionPanel.Instance.HideAnswers();
        AnswerQuestionPanel.Instance.ShowInput();
        AnswerQuestionPanel.Instance.ChangeQuestion("This destiny is not written yet. Write it yourself");

        phase = ConversationalQuestionPhase.QUESTION; 
    }
    
    public void EndRecolectionSendQuestion()
    {
        AnswerQuestionPanel.Instance.HideButtonStop();
        AnswerQuestionPanel.Instance.HideInput();
        NetworkController.Instance.SendWebSocketMessage(7, JsonUtility.ToJson(nextDataToSend)); 
    }

    public void OnSendButton()
    {
        Debug.Log(nextDataToSend._id); 
        string aux = AnswerQuestionPanel.Instance.GetInputText();
        AnswerQuestionPanel.Instance.ClearInput(); 
        if (phase == ConversationalQuestionPhase.QUESTION)
        {
            this.nextDataToSend.question = aux; 
        }
        else if (phase > ConversationalQuestionPhase.QUESTION)
        {
            var answers = new List<string>();
            if (nextDataToSend.answers != null)
            {
                for (int i = 0; i< nextDataToSend.answers.Length; i++)
                {
                    answers.Add(nextDataToSend.answers[i]); 
                }
            }
            answers.Add(aux);
            nextDataToSend.answers = answers.ToArray(); 
        }

        if (phase == ConversationalQuestionPhase.ASK4)
        {
            EndRecolectionSendQuestion(); 
        }

        phase++;
        AnswerQuestionPanel.Instance.ChangeQuestion("Write a potential new destiny"); 
        if (phase > ConversationalQuestionPhase.ASK1)
        {
            AnswerQuestionPanel.Instance.ShowButtonStop(); 
        }
    }
}
