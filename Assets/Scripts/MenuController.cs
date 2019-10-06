using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement; 
public class MenuController : MonoBehaviour
{

    [SerializeField]
    private float animSpeed = 0.6f;
    [SerializeField]
    private TextMeshProUGUI text;
    private bool fadeLoop = true; 
    // Start is called before the first frame update
    void Start()
    {
        LoopAnim(); 
    }

    private void LoopAnim()
    {
        fadeLoop = !fadeLoop;
        text.DOFade(fadeLoop ? 0 : 1, animSpeed).OnComplete(()=>
        {
            LoopAnim(); 
        }); 
    }




    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene(1); 
        }
    }
}
