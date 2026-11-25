using UnityEngine;

public class SpawnScript : MonoBehaviour
{

    [SerializeField] PlayerMovement playerObj;
    [SerializeField] ParticleSystem spawnParticle;
    [SerializeField] ParticleSystem spawnParticleBurst;

    GameObject player;

    bool playerSpawned = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = playerObj.gameObject;
        player.transform.position = transform.position;
        player.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerSpawned)
        {
            if (spawnParticle.isStopped == true)
            {
                playerSpawned = true;
                spawnParticleBurst.Play(true);
                SpawnPlayer();
            }

        }
    }

    void SpawnPlayer()
    {
        // do something here, probably teleport player when they fall instead of instantiating a new one
        player.transform.position = transform.position;
        player.SetActive(true);
    }
}
