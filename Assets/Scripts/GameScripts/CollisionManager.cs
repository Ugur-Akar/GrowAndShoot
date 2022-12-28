using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class CollisionManager : MonoBehaviour
{
    //Settings

    // Connections
    GameObject collectible;
    public GameObject bleedGO;
    public Color playerColor;
    public event Action<float> AddScore;
    public event Action DecreaseScore;
    public event Action OnWallHit;
    // State Variables
    public float score;
    // Start is called before the first frame update
    void Start()
    {
        //InitConnections();
        //InitState();
    }
    void InitConnections(){
    }
    void InitState(){
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.collider.CompareTag("Collectible"))
        {
            collectible = collision.collider.gameObject;

            score = collectible.GetComponent<Collectible>().GetScore();
            AddScore(score);

            Destroy(collectible);
        }
        else if (collision.collider.CompareTag("Wall"))
        {
            OnWallHit();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyBullet"))
        {
            DecreaseScore();
            Destroy(other.gameObject);
            GameObject newBleedGO = Instantiate(bleedGO, transform.position, Quaternion.identity);
            BleedOnHit bleedScript = newBleedGO.GetComponent<BleedOnHit>();
            bleedScript.SetColor(playerColor);
            bleedScript.Bleed();
            
        }
    }
}
