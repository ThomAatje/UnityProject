using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{

    public GameObject PrefabObject;
    public int SpawnCount;
    public GameObject[] SpawnPoints;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
    }

    private void Awake()
    {
        for (int i = 0; i < SpawnCount; i++)
        {
            GameObject spawnpoint = SpawnPoints[Random.Range(0, SpawnPoints.Length)];
            GameObject zombie = Instantiate(PrefabObject, spawnpoint.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
