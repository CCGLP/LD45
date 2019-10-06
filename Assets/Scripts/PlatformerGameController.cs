using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class PlatformerGameController : Singleton<PlatformerGameController>
{
    [SerializeField]
    private float speed = 3;

    [SerializeField]
    private float jumpForce = 400;

    [SerializeField]
    private float gravityScaleFalling = 1.2f; 


    private Rigidbody2D rb;
    private bool canJump = false, canChangeScale = false;

    private float timer = 0;

    [SerializeField]
    private float timeToWait = 14; 
    [SerializeField]
    private GameObject platformPrefab, enemyPrefab;

    private bool chooseSite = false;
    private string inputText;
    private string changeType = "platform";
    private int life = 3; 
    void Start()
    {
        this.rb = this.GetComponent<Rigidbody2D>();
        NetworkController.Instance.SendWebSocketMessage(2, NetworkController.Instance.GameType); 
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, rb.velocity.y);
        if (canJump && Input.GetKeyDown(KeyCode.Space))
        {
            canJump = false;
            rb.AddForce(new Vector2(0, jumpForce)); 
        }
        else if (!canJump && canChangeScale && rb.velocity.y < 0)
        {
            rb.gravityScale = gravityScaleFalling; 
        }

        timer += Time.deltaTime; 

        if ( timer > timeToWait)
        {
            timer = -800;
            NewDestiny(); 

        }


        if (chooseSite)
        {
            if (Input.GetMouseButtonDown(0))
            {
               Vector3 aux =  Camera.main.ScreenToWorldPoint(Input.mousePosition);
               DataPlatformer data = new DataPlatformer { cardType = NetworkController.Instance.GameType, type = changeType, x =(int)aux.x, y = (int)aux.y };
                aux.z = 0; 
                NetworkController.Instance.SendWebSocketMessage(3, JsonUtility.ToJson(data));
                Instantiate(changeType == "platform" ? platformPrefab : enemyPrefab, aux, Quaternion.identity);
                chooseSite = false;
                AnswerQuestionPanel.Instance.HideTooltip();
                AnswerQuestionPanel.Instance.HidePanel(); 
            }
        }
    }

    public void TakeHit()
    {
        life--;
        AnswerQuestionPanel.Instance.TakeHit(); 
        if (life <= 0)
        {
            SceneManager.LoadScene(5); 
        }
    }

    public void SetNewText(string text)
    {
        this.inputText = text;
    }


    public void OnSendClick()
    {
        inputText = AnswerQuestionPanel.Instance.GetInputText(); 
        int res; 
        if (int.TryParse(inputText,out res))
        {

            DataPlatformer data = new DataPlatformer { cardType = NetworkController.Instance.GameType, type = changeType, x = changeType == "jump" ? 0 : res, y = changeType == "jump" ? res : 0 };
            NetworkController.Instance.SendWebSocketMessage(3, JsonUtility.ToJson(data)); 


            if (changeType == "speed")
            {
                speed = res;
            }
            else
            {
                jumpForce = res;
            }
            //ENVIO
            AnswerQuestionPanel.Instance.HidePanel(); 
        }
    }
    private void NewDestiny()
    {
        AnswerQuestionPanel.Instance.ShowPanel(); 
        AnswerQuestionPanel.Instance.ChangeQuestion("Choose the new destiny in this game: ");

        var answers = new List<AnswerInfo>();

        answers.Add(new AnswerInfo
        {
            name = "Platform",
            onClick = () =>
            {
                AnswerQuestionPanel.Instance.HideAnswers();
                chooseSite = true;
                changeType = "platform";
                AnswerQuestionPanel.Instance.ChangeQuestion("Choose where you want to put it"); 
            }
        });

        answers.Add(new AnswerInfo
        {
            name = "Speed",
            onClick = () =>
            {
                changeType = "speed";
                AnswerQuestionPanel.Instance.HideAnswers();
                AnswerQuestionPanel.Instance.ShowInput(); 
            }
        });


        answers.Add(new AnswerInfo
        {
            name = "Jump",
            onClick = () =>
            {
                changeType = "jump";
                AnswerQuestionPanel.Instance.HideAnswers();
                AnswerQuestionPanel.Instance.ShowInput();
            }
        });

        answers.Add(new AnswerInfo
        {
            name = "Enemy",
            onClick = () =>
            {
                AnswerQuestionPanel.Instance.HideAnswers();
                chooseSite = true;
                changeType = "enemy";
                AnswerQuestionPanel.Instance.ChangeQuestion("Choose where you want to put it");

            }
        });

        AnswerQuestionPanel.Instance.ShowAnswers(answers); 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D[] x = new ContactPoint2D[20];
        float yMid = 0;
        int i = 0;
        for (i = 0; i < collision.GetContacts(x); i++)
        {
            yMid += x[i].point.y;
        }
        yMid /= i;
        if (yMid < (this.transform.position.y - this.transform.localScale.y * 0.47f))
        {
            ActiveJump();
        }
    }

    public void InstantiatePlatform(DataPlatformer data)
    {
        Instantiate(platformPrefab, new Vector3(data.x, data.y, 0), Quaternion.identity);
    }

    public void InstantiateEnemy(DataPlatformer data)
    {
        Instantiate(enemyPrefab, new Vector3(data.x, data.y, 0), Quaternion.identity); 
    }

    public void ChangeSpeed(DataPlatformer data)
    {
        speed = data.x;
    }

    public void ChangeJump(DataPlatformer data)
    {
        jumpForce = data.y; 
    }

    private void ActiveJump()
    {
        Debug.Log("Saltando");
        canJump = true;
        rb.gravityScale = 1.0f;
        canChangeScale = true; 
    }
}
