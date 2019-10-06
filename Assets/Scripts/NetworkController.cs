using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NativeWebSocket;
using UnityEngine.SceneManagement; 

public class NetworkController : GlobalSingleton<NetworkController>
{

    [SerializeField]
    private bool local = true;


    private WebSocket websocket;


    async void Start()
    {
        if (local)
        {
            websocket = new WebSocket("ws://localHost:3000");
        }
        else
        {
            websocket = new WebSocket("ws://ec2-13-58-237-21.us-east-2.compute.amazonaws.com:3000");
        }

        websocket.OnOpen += () =>
        {
            Debug.Log("Connection open!");

        };

        websocket.OnError += (e) =>
        {
            Debug.Log("Error! " + e);
        };

        websocket.OnClose += (e) =>
        {
            Debug.Log("Connection closed!");
            
        };

        websocket.OnMessage += (bytes) =>
        {
            var message = System.Text.Encoding.UTF8.GetString(bytes);

            MessageData data = JsonUtility.FromJson<MessageData>(message);
            switch (data.id)
            {
                case 0: //Conexión principal, elegir tipo de juego o preguntar
                    MessageStartGame x = JsonUtility.FromJson<MessageStartGame>(data.message); 
                    switch ((GameType)x.gameType)
                    {
                        case GameType.ASK:
                            InitialGameController.Instance.AskGame(); 
                            break;

                        default:
                            SceneManager.LoadScene(2 + x.gameType);
                            break;
                    }
                    break;

                case 1:
                    break;

                case 2:

                    break; 
            }
            Debug.Log("OnMessage! " + data.id + " " + data.message);
        };

       

        await websocket.Connect();

    }


    private void OnApplicationQuit()
    {
        SendWebSocketMessage(-1, "quit"); 
    }

    public async void SendWebSocketMessage(int id, string message)
    {
        if (websocket.State == WebSocketState.Open)
        {
            MessageData data = new MessageData{ id = id, message = message };
            

            // Sending plain text
            await websocket.SendText(JsonUtility.ToJson(data));
        }
    }

}
