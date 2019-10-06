using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro; 

public class AnswerQuestionPanel : Singleton<AnswerQuestionPanel>
{
    [SerializeField]
    private CanvasGroup cg, cgAnswers, cgInput,cgHit, cgButtonStop;

    [SerializeField]
    private TextMeshProUGUI question, tooltip;

    [SerializeField]
    private GameObject parentAnswer, answerPrefab;

    [SerializeField]
    private TMP_InputField input; 
    

    void Start()
    {
        
    }

    public void ClearAnswers()
    {
        int x = parentAnswer.transform.childCount; 
        for (int i = 0; i< x; i++)
        {
            Destroy(parentAnswer.transform.GetChild(i).gameObject); 
        }
    }

    public void ShowButtonStop()
    {
        cgButtonStop.interactable = true;
        cgButtonStop.blocksRaycasts = true;
        cgButtonStop.DOFade(1, 0.5f);
    }

    public void HideButtonStop()
    {
        cgButtonStop.interactable = false;
        cgButtonStop.blocksRaycasts = false;
        cgButtonStop.DOFade(0, 0.5f);
    }

    public void TakeHit()
    {
        Camera.main.DOShakePosition(0.3f); 
        cgHit.DOFade(1, 0.4f).OnComplete(() =>
        {
            cgHit.DOFade(0, 0.4f); 
        });
    }

    public void ShowPanel()
    {
        cg.interactable = true;
        cg.blocksRaycasts = true;
        cg.DOFade(1, 0.5f); 
    }


    public void HidePanel()
    {
        cg.interactable = false;
        cg.blocksRaycasts = false;
        cg.DOFade(0, 0.5f); 
    }

    public void ClearInput()
    {
        input.text = ""; 
    }
    public void ShowInput()
    {
        cgInput.interactable = true;
        cgInput.blocksRaycasts = true;
        cgInput.DOFade(1, 0.5f);
    }

    public void HideInput()
    {
        cgInput.interactable = false;
        cgInput.blocksRaycasts = false;
        cgInput.DOFade(0, 0.5f);
    }

    public void HideAnswers()
    {
        cgAnswers.interactable = false;
        cgAnswers.blocksRaycasts = false;
        cgAnswers.DOFade(0, 0.5f); 
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

    public void ShowTooltip()
    {
        tooltip.DOFade(1, 0.5f);
    }
    public void HideTooltip()
    {
        tooltip.DOFade(0, 0.5f); 
    }
    public string GetInputText()
    {
        return input.text; 
    }
    void Update()
    {
        
    }
}
