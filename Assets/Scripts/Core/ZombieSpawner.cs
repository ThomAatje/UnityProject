using Assets.Scripts.Actor;
using System;
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
    private int CurrentZombieCount = 0;

    [Header("DEBUG")]
    private float DebugDamageTime = 0;

    private GameObject TargetPlayer;

    private List<GameObject> Zombies = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
    }

    private void Awake()
    {
        TargetPlayer = GameObject.FindGameObjectWithTag("Player");
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
        if (CurrentZombieCount < MAX_ZOMBIE_COUNT)
        {
            CurrentSpawnTime += Time.deltaTime;

            if (CurrentSpawnTime >= SpawnDelay)
            {
                int rnd = UnityEngine.Random.Range(0, SpawnPoints.Length);
                Zombies.Add(Instantiate(PrefabObject, SpawnPoints[rnd].transform));
                // zombie stuff toevoegen

                CurrentSpawnTime = 0;
                CurrentZombieCount++;
            }
        }
    }

    void ZombieUpdate()
    {
        DebugDamageTime += Time.deltaTime;

        if (DebugDamageTime >= SpawnDelay)
        {
            for (int i = 0; i < Zombies.Count; i++)
            {
                GameObject zombie = Zombies[i];
                Zombie zombieScript = zombie.GetComponent<Zombie>();

                if (zombieScript.IsDead)
                {
                    DestroyZombie(zombie);
                    CurrentZombieCount--;
                    Zombies.Remove(zombie);
                }
            }

            DebugDamageTime = 0;
        }
    }

    public void DestroyZombie(GameObject zombie)
    {
        Destroy(zombie);
    }
}
