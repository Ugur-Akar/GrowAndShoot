using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GateCollider : MonoBehaviour
{
    //Settings

    // Connections
    public GameObject sprite;
    public Transform zPoint;
    public List<GameObject> splashes;
    // State Variables
    Color bulletColor;
    bool splashAllowed;
    // Start is called before the first frame update
    void Start()
    {
        //InitConnections();
        InitState();
    }
    void InitConnections(){
    }
    void InitState(){
        splashAllowed = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AddSplash(Vector3 pos)
    {
        if (splashAllowed)
        {
            GameObject splashGO = Instantiate(sprite, pos, Quaternion.identity);
            splashGO.transform.position = new Vector3(splashGO.transform.position.x, splashGO.transform.position.y, zPoint.transform.position.z);
            splashGO.GetComponent<SpriteRenderer>().color = bulletColor;
            splashes.Add(splashGO);
            SplashEffect splashEffect = splashGO.GetComponent<SplashEffect>();
            splashEffect.Randomize();
            splashGO.transform.parent = transform;
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            bulletColor = other.GetComponent<Renderer>().material.color;
            AddSplash(other.transform.position);
            Destroy(other.gameObject);
        }
    }

    public void DestroySplash()
    {
        splashAllowed = false;
        foreach (GameObject splash in splashes)
        {         
            Destroy(splash);
        }
        splashes.Clear();
    }
}
