using System;
using System.Collections;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Rigidbody2D rb;
    void Start()
    {
        // The body is first Kinematic so the object cannot be moved by gravity
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        //checks if the collision is with player and plays the Coroutine
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Falling());
        }
        
    }

    //waits 3 seconds before changing the body type to dynamic so that the body can start falling
    public IEnumerator Falling()
    {
        Debug.Log("Started falling coroutine");
        yield return new WaitForSeconds(3f);
        Debug.Log("Gravity enabled");
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 2f;
        Debug.Log(rb.gravityScale);

    }


}
