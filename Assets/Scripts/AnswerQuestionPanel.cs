using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro; 

public class AnswerQuestionPanel : Singleton<AnswerQuestionPanel>
{
    [SerializeField]
    private CanvasGroup cg, cgAnswers;

    [SerializeField]
    private TextMeshProUGUI question;

    [SerializeField]
    private GameObject parentAnswer, answerPrefab;

    void Start()
    {
        
    }

    public void ChangeQuestion(string text)
    {
        question.text = text;
    }
    public void ShowAnswers(List<AnswerInfo> answers)
    {
        Debug.Log("Hola"); 
        cgAnswers.blocksRaycasts = true;
        cgAnswers.interactable = true;
        cgAnswers.DOFade(1, 0.5f); 

        for (int i = 0; i<answers.Count; i++)
        {
            var go = (GameObject) Instantiate(answerPrefab, parentAnswer.transform);
            go.GetComponent<Answer>().Init(answers[i]); 
        }
    }

    void Update()
    {
        
    }
}
