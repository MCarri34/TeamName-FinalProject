using System.Collections;
using UnityEngine;

public class SpawnScript : MonoBehaviour
{

    public static SpawnScript Instance { get; private set; }

    [SerializeField] PlayerMovement playerObj;
    [SerializeField] ParticleSystem spawnParticle;
    [SerializeField] ParticleSystem spawnParticleBurst;

    GameObject player;

    bool playerSpawned = false;

    private void Awake()
    {
        // Destroy duplicate instances if one already exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Optional: Keep this object alive when loading new scenes
        DontDestroyOnLoad(gameObject);
    }

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

    public void SpawnPlayer()
    {
        // do something here, probably teleport player when they fall instead of instantiating a new one
        player.transform.position = transform.position;
        player.SetActive(true);
    }

    public void KillPlayer()
    {
        player.transform.position = transform.position;
        player.SetActive(false);
        StartCoroutine(spawnDelay());
    }

    IEnumerator spawnDelay()
    {
        yield return new WaitForSeconds(0.5f);
        playerSpawned = false;
        spawnParticle.Play(true);
    }
}
