using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformEnemy : MonoBehaviour
{
    [SerializeField]
    private float speed = 8;
    private int dir = 1; 
    private Rigidbody2D rb; 
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(speed, 0); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        dir *= -1;
        rb.velocity = new Vector2(speed * dir, 0);
        if (collision.gameObject.tag == "Player")
        {
            PlatformerGameController.Instance.TakeHit(); 
        }
    }
}
