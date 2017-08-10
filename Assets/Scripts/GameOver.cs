using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{

    public GameObject[] gameOverLights;
    public GameObject mainLight;
    public GameObject[] gameOverPanels;
    public GameObject spawnPoints;
    public GameObject pointerLight;

    public void GameOverSequence()
    {
        // stop spawning
        StopSpawning();
        // detroy all active enemies in scene
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
        // red spotlight
        ChangeLight();
        // activate game over screens
        ActivatePanels();
    }

    void ChangeLight()
    {
        mainLight.SetActive(false);
        pointerLight.SetActive(false);
        foreach(GameObject light in gameOverLights)
        {
            light.SetActive(true);
        }
    }

    void ActivatePanels()
    {
        foreach (GameObject g in gameOverPanels)
        {
            g.SetActive(true);
        }
    }

    void StopSpawning()
    {
        spawnPoints.GetComponent<EnemySpawn>().enabled = false;
    }
}
