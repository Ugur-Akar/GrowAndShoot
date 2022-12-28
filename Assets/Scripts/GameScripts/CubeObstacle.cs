using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CubeObstacle : MonoBehaviour
{
    //Settings
    public int health;
    // Connections
    public GameObject splashSprite;
    public GameObject otherCube;
    Material bulletMaterial;
    Transform zPos;
    SwellEffect swellEffect;
    public Action OnExploded;

    public event Action OnCollisionWithPlayer;
    public event Action WallExploded;
    // State Variables
    Vector3 splashPos;
    bool isExploded;
    bool didntTouchPlayer = true;
    // Start is called before the first frame update
    void Start()
    {
        InitConnections();
        InitState();
    }
    void InitConnections()
    {
        zPos = transform.GetChild(0).transform; // TODO: Bu tur su kadarinci cocugu bu parent'i vs. baglantilardan kacinalim.
        swellEffect = GetComponent<SwellEffect>();
    }
    void InitState()
    {
        isExploded = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(health <= 0 && !isExploded)
        {
            if (didntTouchPlayer)
            {
                WallExploded();
            }

            Explode();
        }
    }

    void Explode()
    {        
        GetComponent<MeshRenderer>().enabled = false;
        Instantiate(otherCube, transform.position, transform.rotation);
        isExploded = true;
        OnExploded?.Invoke();
        Destroy(gameObject);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            bulletMaterial = other.GetComponent<Renderer>().material;
            splashPos = other.transform.position;
            splashPos.z = zPos.transform.position.z;
            Destroy(other.gameObject);
            health--;
            AddSplashEffect();
            swellEffect.ApplyEffect(shrinkBack: true);
        }
        else if (other.CompareTag("EnemyBullet"))
        {
            Destroy(other.gameObject);
        }
    }

    void AddSplashEffect()
    {
        Color splashColor = bulletMaterial.color;
        GameObject newSplashGO = Instantiate(splashSprite, splashPos, Quaternion.identity);
        newSplashGO.GetComponent<SpriteRenderer>().color = splashColor;
        newSplashGO.transform.parent = transform;
        SplashEffect newSplash = newSplashGO.GetComponent<SplashEffect>();
        newSplash.Randomize();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Player"))
        {
            didntTouchPlayer = false;
            health = 0;
        }
    }

    
}
