using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BulletManager : MonoBehaviour
{
    //Settings
    public float speed;
    public bool isEnemy;
    public float timeLimit;
    public float initialScale = 0.2f;
    public float lastScale = 0.4f;
    public float growTime = 3;
    public float launchTime = 1.0f;
    // Connections
    public Transform bulletParent;
    public Transform endTarget;
    public SwellEffect swellEffect;
    TrailRenderer trailRenderer;
    // State Variables
    public Vector3 vectorToPlayer;
    float timeSinceStart;
    bool isDestroyed;
    public bool isLaunched;
    public bool shootAtEndTarget = false;
    // Start is called before the first frame update
    void Start()
    {
        InitConnections();
        InitState();
    }
    void InitConnections(){
        swellEffect = GetComponent<SwellEffect>();
        trailRenderer = GetComponent<TrailRenderer>();
    }
    void InitState(){
        timeSinceStart = 0;
       // transform.localScale = initialScale;
       // transform.DOScale(lastScale , timeLimit).SetEase(Ease.OutElastic);
        isLaunched = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isLaunched)
        {
            timeSinceStart += Time.deltaTime;
            if (!isEnemy)
            {
                if (shootAtEndTarget)
                {
                    Vector3 offset = endTarget.position - transform.position;
                    offset /= offset.magnitude;
                    transform.Translate(offset * speed * Time.deltaTime,Space.World);
                }
                else
                {
                    transform.Translate(Vector3.forward * speed * Time.deltaTime,Space.World);
                }
            }
            else
            {
                transform.Translate(vectorToPlayer * speed * Time.deltaTime);
            }
        }
        

        if(timeSinceStart >= timeLimit && !isDestroyed)
        {
           // transform.DOScale(0, 0.5f).SetEase(Ease.InCirc);
            Destroy(gameObject,0.5f);
            isDestroyed = true;
        }
    }
    public void Launch(Transform source, Transform destination)
    {
        //transform.position = source.position;
        //Tween launchTween = transform.DOMoveY(destination.position.y, launchTime);
        //launchTween.OnComplete(OnLaunched);
        Launch(new Transform[] { source,destination});
    }

    public void Launch(Transform[] wayPoints)
    {
        Vector3[] wayPointLocalPositions = new Vector3[wayPoints.Length];
        for(int i=0; i< wayPointLocalPositions.Length; i++)
        {
            wayPointLocalPositions[i] = transform.parent.InverseTransformPoint(wayPoints[i].position);
        }
        transform.DOLocalPath(wayPointLocalPositions, launchTime, PathType.Linear).OnComplete(OnLaunched);
        
     
    }

    void OnTravelEnd()
    {
        transform.DOScale(lastScale, growTime).OnComplete(OnLaunched).SetEase(Ease.OutBounce);
    }

    void OnLaunched()
    {
        trailRenderer.enabled = true;
        swellEffect.Oscillate();
        transform.parent = bulletParent; 
        isLaunched = true;
    }
}
