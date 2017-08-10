using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Since this script is instantiated on all spawn points, they spawn enemies all at the same times.
// to randomize both spawn time and spawn points, need to attach this script to one instance of a gameobject
// and have a reference to all the spawn points available.

public class EnemySpawn : MonoBehaviour
{
    public GameObject player;
    GameObject[] spawnPoints;
    int spawnIndex = 0;
    string currentLevel;
    public GameObject enemy1Prefab;
    public GameObject enemy2Prefab;
    float myTime = 0f;

    GameObject theEnemy;

    float minTime = 1;
    float maxTime = 2;

    float spawnTime;

    // Use this for initialization
    void Start()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("spawn");
        SetRandomSpawnTime();
    }

    // Update is called once per frame
    void Update()
    {
        myTime += Time.deltaTime;
        // need to randomize when enemies are called from different spawn points
        if (spawnPoints[spawnIndex].GetComponent<SpawnPtProperties>().aliveEnemies < 5 && myTime >= spawnTime)
        {
            SpawnEnemy(enemy1Prefab);
            SetRandomSpawnTime();
            myTime = 0f;
        }
    }

    void SetRandomSpawnTime()
    {
        spawnIndex = (int)Random.Range(0, spawnPoints.Length);
        spawnTime = Random.Range(minTime, maxTime);
    }

    void SpawnEnemy(GameObject enemy)
    {
        theEnemy = Instantiate(enemy, spawnPoints[spawnIndex].transform.position, enemy.transform.rotation);
        theEnemy.transform.LookAt(player.transform);
        // this finds the reference to the specific spawn point -- if a spawn point spawns 5 enemies and they
        // are all still active, it will not spawn any more until conditions change.
        theEnemy.GetComponent<BasicEnemy>().spawnPt = spawnPoints[spawnIndex].GetComponent<SpawnPtProperties>();
        ++(theEnemy.GetComponent<BasicEnemy>().spawnPt.aliveEnemies);
    }
}
