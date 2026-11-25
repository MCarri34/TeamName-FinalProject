using UnityEngine;

public class KillAreaScript : MonoBehaviour
{
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("obj entered");
        if (other.CompareTag("Player"))
        {
            Debug.Log("player killed");
            SpawnScript.Instance.KillPlayer();
        }
    }
}
