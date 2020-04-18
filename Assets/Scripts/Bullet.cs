using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{


    #region Attributes
    public Rigidbody2D rb;
    public float speed = 15f;
    public ParticleSystem ps;
    GameManager gm;
    MainMenuManager mm;
    #endregion

    #region Events
    public event OnHitEventDelegate OnLetterHitEvent;
    public delegate void OnHitEventDelegate(GameObject bullet, GameObject hitted);
    #endregion

    void Start()
    {
        try
        {
            gm = GameObject.Find("GameManager").GetComponent<GameManager>();
            OnLetterHitEvent += gm.OnLetterHit;

        }
        catch 
        {
            mm = Camera.main.GetComponent<MainMenuManager>();
        }
        rb.velocity = transform.up * speed;
    }
    

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.name == "TouchableLetterField(Clone)")
        {
            if (OnLetterHitEvent != null)
            {
                GetComponent<CircleCollider2D>().isTrigger = false;
                OnLetterHitEvent.Invoke(gameObject, collision.gameObject);
                OnLetterHitEvent -= gm.OnLetterHit;
            }
        }
        print(collision.gameObject.name);
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Destroy(gameObject);
    }
}
