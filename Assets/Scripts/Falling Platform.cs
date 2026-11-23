using System;
using System.Collections;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField] ParticleSystem revealParticle;
        
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Rigidbody2D rb;
    
    SpriteRenderer sprite;

    void Start()
    {
        // The body is first Kinematic so the object cannot be moved by gravity
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0f;

        sprite = GetComponent<SpriteRenderer>();
        //revealParticle.Play(false);
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
            revealParticle.Play(true);
            StartCoroutine(Falling());
        }
        
    }

    //waits 3 seconds before changing the body type to dynamic so that the body can start falling
    public IEnumerator Falling()
    {
        Debug.Log("Started falling coroutine");
        Color currCol = sprite.color;

        while(currCol.a != 0f)
        {
            if (currCol.a <= 0.2f)
            {
                break;
            }
            currCol.a -= 0.1f;
            sprite.color = currCol;
            yield return new WaitForSeconds(0.25f);
        }

        yield return new WaitForSeconds(0.1f);
        revealParticle.Play(false);
        Debug.Log("Gravity enabled");
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 2f;
        Debug.Log(rb.gravityScale);

    }


}
