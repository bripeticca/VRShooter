using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Defines all actions available to a player. Keeps track of health and score.

public class PlayerActions : MonoBehaviour
{
    // ~~~~~~~Weapon Variables~~~~~~~~~
    [Header("Weapon Variables")]
    public GameObject weaponPrefab;
    GameObject weaponInstance;
    // ~~~~~~~~Health and Points~~~~~~~
    [Header("Health and Points")]
    public Text HP;
    public Text score;
    static int health = 100;
    static int points = 0;
    // ~~~~~~~Other ~~~~~~~~~~~~~~~~~~
    RaycastHit hit;
    GameObject enemy;
    GameOver gameOver;

    // ~~~~~~~~~~ Controller ~~~~~~~~~
    private SteamVR_TrackedObject trackedObj;
    private SteamVR_Controller.Device controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }
    // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    // Use this for initialization
    void Start()
    {
        gameOver = GetComponent<GameOver>();
    }

    // Update is called once per frame
    void Update()
    {
        HP.text = "HP: " + health.ToString();
        score.text = "Score: " + points.ToString();

        if (controller.GetHairTriggerDown())
            Fire();

        if (health <= 0)
        {
            gameOver.GameOverSequence();
        }

        else if (Physics.Raycast(controller.transform.pos, transform.forward, out hit) && hit.collider.tag == "enemy")
        {
            enemy = hit.collider.gameObject;
            enemy.GetComponent<BasicEnemy>().ShowStats();
        }
        else if (enemy)
            enemy.GetComponent<BasicEnemy>().HideStats();
    }

    // Called when user fires their current weapon
    // NOTE: NEED TO CHANGE THIS TO BE ABLE TO DYNAMICALLY CHANGE WEAPONS
    void Fire()
    {
        weaponInstance = Instantiate(weaponPrefab, this.transform.position, this.transform.rotation);
        weaponInstance.GetComponent<Rigidbody>().velocity = weaponInstance.transform.forward * 6;
        Destroy(weaponInstance, 5.0f);
    }

    // Called when player gets hit by enemy
    static public void Hit(int damage)
    {
        health -= damage;
    }

    // Called when player kills an enemy
    static public void AddPoints(int worth)
    {
        points += worth;
    }
}
