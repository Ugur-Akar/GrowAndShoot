using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class BleedOnHit : MonoBehaviour
{

    public const float FULL_ROTATION = 360;

    //Settings
    public float radius;
    public float forceCoef = 1;
    public float jumpDuration = 1.5f;
    public float minScale;
    public float maxScale;
    public float lifeTime = 3.0f;
    // Connections
    public GameObject splashGO;
    Renderer thisRenderer;
    // State Variables
    Vector2 throwVector;
    Vector3 endPosition;
    // Start is called before the first frame update
    void Start()
    { 
        InitConnections();
        InitState();
    }
    void InitConnections(){
        thisRenderer = GetComponent<Renderer>();
    }
    void InitState(){
        throwVector = new Vector2(transform.position.x + Random.Range(-radius, radius), transform.position.y + Random.Range(-radius, radius));
        endPosition = new Vector3(throwVector.x, 0, throwVector.y);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetColor(Color color)
    {
        if(thisRenderer == null)
        {
            thisRenderer = GetComponent<Renderer>();
            throwVector = new Vector2(transform.position.x + Random.Range(-radius, radius), transform.position.z + Random.Range(-radius, radius));
            endPosition = new Vector3(throwVector.x, 0, throwVector.y);
        }
        thisRenderer.material.color = color;
    }
    public void Bleed()
    {
        transform.DOJump(endPosition, forceCoef, 0, jumpDuration);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            Splash();
            thisRenderer.enabled = false;
        }
    }

    void Splash()
    {
        GameObject newSplash = Instantiate(splashGO, transform.position, Quaternion.identity);
        newSplash.GetComponent<SpriteRenderer>().color = thisRenderer.material.color;
        float yRotation = Random.Range(0, FULL_ROTATION);
        float size = Random.Range(minScale, maxScale);
        newSplash.transform.DORotate(new Vector3(90, yRotation, 0), 0.1f, RotateMode.Fast);
        newSplash.transform.DOScale(transform.localScale * size,0.5f);
        newSplash.GetComponent<SpriteRenderer>().DOFade(0.3f, lifeTime);
       // newSplash.transform.localScale = Random.Range(minScale, maxScale) * newSplash.transform.localScale; // TODO Magic number for test
        //newSplash.transform.localScale = Random.Range(1.0f, 2.0f) * newSplash.transform.localScale; // TODO Magic number for test


    }
}
