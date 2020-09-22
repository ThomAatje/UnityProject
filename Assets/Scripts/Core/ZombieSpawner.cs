using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public int MAX_ZOMBIE_COUNT = 15;
    public float SpawnDelay = 1;

    public GameObject PrefabObject;
    public GameObject[] SpawnPoints;

    private float CurrentSpawnTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
    }

    private void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SpawnUpdate();
        ZombieUpdate();
    }

    // Game updates
    void SpawnUpdate()
    {
        CurrentSpawnTime += Time.deltaTime;
    }

    void ZombieUpdate()
    {

    }
}
