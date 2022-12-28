using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using System;

public class EnemyControl : MonoBehaviour
{
    //Settings
    public int health;
    public float delayBetweenBullets;
    float bulletPosOffset;
    public float timeLimit;
    public float range;
    public float throwForceSpan = 1000; 
    // Connections
    public Transform player;
    public GameObject bullet;
    public GameObject bleedGO;
    public Renderer thisRenderer;
    public ColorChanger colorChanger;
    Animator anim;
    RagdollManager ragdollManager;
    public Rigidbody rbToThrow;
    Collider collider;
    public event Action IncreasePoints;
    //public Rig enemyRig;
    // State Variables
    Vector3 bulletPos;
    bool canFire;
    float timeSinceFired;
    [SerializeField]
    bool isInLevel;
    // Start is called before the first frame update
    void Start()
    {
        InitConnections();
        InitState();
        
    }
    void InitConnections(){
        anim = GetComponent<Animator>();
        ragdollManager = GetComponent<RagdollManager>();
        collider = GetComponent<Collider>();
        colorChanger.thisRenderer = thisRenderer;
    }
    void InitState(){
        
        timeSinceFired = 0;
        bulletPosOffset = 1;
        //isInLevel = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isInLevel && health > 0)
        {
            transform.LookAt(player);

            bulletPos = player.position - transform.position;
            bulletPos = bulletPos / bulletPos.magnitude;
            bulletPos.y += bulletPosOffset;
            if (player.transform.position.z < transform.position.z && player.transform.position.z + range >= transform.position.z)
            {
                FireAtPlayer();
            }
        }
        else if(isInLevel && health <= 0)
        {
            //anim.SetBool("Death",true);
            collider.enabled = false;
            Debug.Log("Enemy Died");
            isInLevel = false;
            IncreasePoints();
            //enemyRig.weight = 0;
        }
    }

    void FireAtPlayer()
    {
        timeSinceFired += Time.deltaTime;
        if(timeSinceFired >= delayBetweenBullets)
        {
            timeSinceFired = 0;
            canFire = true;
        }
        if (canFire)
        {
            Vector3 vector3 = player.transform.position - transform.position;
            vector3 = vector3 / vector3.magnitude;
            GameObject enemyBullet = Instantiate(bullet, transform.position + bulletPos, Quaternion.identity);
            EnemyBulletManager bManager = enemyBullet.GetComponent<EnemyBulletManager>();
            bManager.vectorToPlayer = vector3;
            bManager.timeLimit = timeLimit;
            canFire = false;
        }
        
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet")){
            Debug.Log("I got shot!");
            Destroy(other);
            health--;
            Bleed();
            if(health <= 0)
            {
                OnKilled();      
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            OnKilled();
        }
    }

    public void SetInLevel()
    {
        isInLevel = true;
    }

    public void OnKilled()
    {
        anim.enabled = false;
        ragdollManager.SetRagdollEnabled(true);

        colorChanger.ChangeColor(new Vector3(0.5f, 0.5f, 0.5f));

        Vector3 throwForce = new Vector3(
                UnityEngine.Random.Range(0, throwForceSpan),
                UnityEngine.Random.Range(-throwForceSpan, throwForceSpan),
                UnityEngine.Random.Range(-throwForceSpan, throwForceSpan)
            );

        
        rbToThrow.AddForce(throwForce); 
    }

    void Bleed()
    {
        GameObject newBleedGO = Instantiate(bleedGO, transform.position, Quaternion.identity);
        BleedOnHit bleedOnHit = newBleedGO.GetComponent<BleedOnHit>();
        bleedOnHit.SetColor(thisRenderer.material.color);
        bleedOnHit.Bleed();
    }
}
