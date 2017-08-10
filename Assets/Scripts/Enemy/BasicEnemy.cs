using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using UnityEngine.VR;
using UnityEngine.UI;

// Base class for all enemy types -- this script is also used 

public class BasicEnemy : MonoBehaviour
{
    // =========================================
    // |--------------Variables----------------|
    // =========================================

    // ~~~~~~~~~~~ Positions ~~~~~~~~~~~~~~
    [Header("Positions")]
    protected Vector3 playerPos;
    public SpawnPtProperties spawnPt;
    // ~~~~~~~~~ Enemy Objects ~~~~~~~~~~~~
    [Header("Enemy Objects")]
    public ParticleSystem explode;
    public Text stats;
    public Material hilite;
    public GameObject weaponPrefab;
    // ~~~~~~~~~~ Time ~~~~~~~~~~~~~~~~~~~~
    float myTime;
    float shootTime;
    float min = 2f;
    float max = 5f;
    // ~~~~~~~~ Health and Damage ~~~~~~~~~
    int health = 3;
    int damage = 1;
    // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

    // Use this for initialization
    void Start()
    {
        playerPos = UnityEngine.VR.InputTracking.GetLocalPosition(VRNode.Head);
        SetRandomTime();
    }

    protected void TakeHit(int attack)
    {
        health -= attack;
        spawnPt.aliveEnemies -= 1;
    }

    void OnCollisionEnter(Collision other)
    {
        TakeHit(other.gameObject.GetComponent<WeaponAction>().Damage());
    }

    public void ShowStats()
    {
        stats.gameObject.SetActive(true);
        stats.text = "Health: " + health + "\nDamage: " + damage;
        Highlight();
    }

    public void HideStats()
    {
        stats.gameObject.SetActive(false);
        Unhighlight();
    }

    protected void Highlight()
    {
        List<Material> mats = new List<Material>(this.GetComponent<Renderer>().materials);
        mats.Add(hilite);
        GetComponent<Renderer>().materials = mats.ToArray();
    }

    protected void Unhighlight()
    {
        List<Material> mats = new List<Material>(this.GetComponent<Renderer>().materials);
        mats.Remove(mats[1]);
        GetComponent<Renderer>().materials = mats.ToArray();
    }

    // NOTE TO SELF: for some reason the weapon prefab is not instantiating ?
    protected virtual void Shoot()
    {
        GameObject weapon;
        weapon = Instantiate(weaponPrefab, this.transform.position, this.transform.rotation);
        Debug.Log("weapon: " + weapon);
        weapon.GetComponent<Rigidbody>().velocity = weapon.transform.forward * 6;
        Destroy(weapon, 5.0f);
    }

    protected void SetRandomTime()
    {
        shootTime = Random.Range(min, max);
        myTime = 0.0f;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        myTime += Time.deltaTime;

        if (health <= 0)
        {
            explode.gameObject.SetActive(true);
            Destroy(gameObject);
            PlayerActions.AddPoints(damage);
        }

        if (transform.position == playerPos)
        {
            spawnPt.aliveEnemies -= 1;
            PlayerActions.Hit(damage);
            Destroy(gameObject);
        }

        if (myTime >= shootTime)
        {
            Shoot();
            SetRandomTime();
        }

        // does this make sure that enemy is always following player position?
        transform.position = Vector3.MoveTowards(transform.position, playerPos, Time.deltaTime);
    }
}
