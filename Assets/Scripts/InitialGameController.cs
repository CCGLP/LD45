using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using TMPro;
using UnityEngine.SceneManagement; 
public class InitialGameController : Singleton<InitialGameController>
{

    [SerializeField]
    private List<GameObject> cards;
    [SerializeField]
    private GameObject highlightCardSpace; 
    private List<Vector3> cardsOriginPosition;  
    [SerializeField]
    private float animCardTime = 0.4f;

    private bool endLoop = false;
    private string cardName; 
    void Start()
    {
        cardsOriginPosition = (from card in cards where card != null select card.transform.position).ToList();

        CardAnim();

        DOVirtual.DelayedCall(Random.Range(5, 8), () =>
        {
            endLoop = true;
        });
    }


    private void CardAnim()
    {
        if (!endLoop)
        {
            for (int i = 0; i < cards.Count; i++)
            {
                cards[i].transform.DOMove(cardsOriginPosition[GetRandomInCardsExcept(i)], animCardTime); 
            }
            DOVirtual.DelayedCall(animCardTime, () =>
            {
                CardAnim(); 
            });

        }
        else   
        {
            int x = 0;

            if (!PlayerPrefs.HasKey("destiny"))
            {
                x = Random.Range(0, cards.Count);
                PlayerPrefs.SetInt("destiny", x); 
            }
            else
            {
                x = PlayerPrefs.GetInt("destiny"); 
            }
            for (int i = 0; i< cards.Count; i++)
            {
                if (i != x)
                {
                    cards[i].GetComponent<Renderer>().material.DOFade(0, animCardTime); 
                }
            }
            cards[x].transform.DOMove(highlightCardSpace.transform.position, animCardTime * 2).OnComplete(()=>
            {
                DecideDestiny(x); 
            }); 
        }
    }

    private void DecideDestiny(int cardNumber)
    {
        cardName = "";
        //cardNumber = 4; //QUITAR CARLOS QUITALO OSTIA
        switch (cardNumber)
        {
            case 0:
                cardName = "fool";
                break;
            case 1:
                cardName = "moon";
                break;
            case 2:
                cardName = "papise"; 
                break;
            case 3:
                cardName = "justice"; 
                break;
            case 4:
                cardName = "hanged"; 
                break; 
        }

        

        NetworkController.Instance.SetGameType(cardName);
        NetworkController.Instance.SendWebSocketMessage(0, cardName); 
        
    }


    public void AskGame()
    {
        AnswerQuestionPanel.Instance.ChangeQuestion("This destiny is not created. Choose the type of game you want to start.");
        List<AnswerInfo> answers = new List<AnswerInfo>();
        answers.Add(new AnswerInfo
        {
            name = "Platformer",
            onClick = () =>
            {
                var message = new MessageDecideGame { cardName = cardName, gameType = "Platformer" };
                NetworkController.Instance.SendWebSocketMessage(1, JsonUtility.ToJson(message));
                SceneManager.LoadScene(2); 
            }
        });

        //answers.Add(new AnswerInfo
        //{
        //    name = "Action",
        //    onClick = () =>
        //    {
        //        var message = new MessageDecideGame { cardName = cardName, gameType = "Action" };
        //        NetworkController.Instance.SendWebSocketMessage(1, JsonUtility.ToJson(message));
        //        SceneManager.LoadScene(3); 
        //    }
        //});


        answers.Add(new AnswerInfo
        {
            name = "Narrative",
            onClick = () =>
            {
                var message = new MessageDecideGame { cardName = cardName, gameType = "Narrative" };
                NetworkController.Instance.SendWebSocketMessage(1, JsonUtility.ToJson(message));
                SceneManager.LoadScene(4); 
            }
        });

        AnswerQuestionPanel.Instance.ShowAnswers(answers); 
    }

    private int GetRandomInCardsExcept(int i)
    {
        int x = i; 
        while (x == i)
        {
            x = Random.Range(0, cards.Count); 
        }
        return x; 
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
