using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using Dreamteck.Splines;
using TMPro;
using System;
using DG.Tweening;



public class PlayerManager : MonoBehaviour
{
    const float BLOOD_FX_LIFETIME = 1.5f;

    //Settings
    public float diminishRate;
    public float jumpBackAmount;
    public float runSpeed, crawlSpeed, hopSpeed;
    public float bulletDamage;
    public float wallDamage;
    float initialTipHeight;
    float currentHeight;
    float heightDifference;
    public float delayBetweenBullets;
    public float bulletForce;
    public float runToCrawlTh;
    public float CrawlToHopTh;
    public Vector3 backforceVector;
    public float levelEndBulletTimeLimit;
    public float minScore;
    public float maxScore;
    public bool isInLevelEnd;
    public float fallbackAnimLength;
    // Connections
    public Scaler[] bodyParts;
    public Transform footTip;
    public Transform hipPoint;
    public Animator playerAnimator;
    public SkinnedMeshRenderer skin;
    public Transform[] leftBulletPath;
    public Transform[] rightBulletPath;
    public Transform[] headBulletPath;
    public Transform endTarget;

    public Transform bulletParent;
    public GameObject bullet;
    public Rig handAim;
    Material playerMat;
    public GameObject character;
    //public GameObject bloodFXGO;

    CollisionManager colManager;
    SplineFollower follower;
    PlayerSplineControl pSpline;

    public event Action ScoreIsZero;
    // State Variables
    public float score;
    bool canFire;
    float timeSinceFired;
    bool isFalling;
    public bool isInLevel;
    float initialHipHeight;
    bool[] animations;
    bool endLevelCanFire;
    public bool shootAtEndTarget = false;
    public bool increseBulletTime = false;
    // Start is called before the first frame update
    void Start()
    {
        InitConnections();
        InitState();

    }
    void InitConnections(){
        pSpline = GetComponent<PlayerSplineControl>();
        follower = GetComponent<SplineFollower>();
        
        colManager = GetComponent<CollisionManager>();

        colManager.AddScore += IncreaseScore;
        colManager.DecreaseScore += TakeDamage;
        colManager.OnWallHit += OnWallHit;
    }
    void InitState(){
        
        initialTipHeight = footTip.position.y;
        initialHipHeight = hipPoint.position.y;
        timeSinceFired = 0;

        animations = new bool[5];
        isFalling = false;
        //playerAnimator.SetFloat("FallbackTime", fallbackAnimLength);

        playerMat = character.GetComponent<Renderer>().material;
        colManager.playerColor = playerMat.color;
        endLevelCanFire = true;
    }

    // Update is called once per frame
    void Update()
    {
        
        //Debug.Log(score);
        

        if (isInLevel)
        {
            score -= diminishRate * Time.deltaTime;

            if (score > maxScore)
            {
                score = maxScore;
            }
            else if (score < minScore)
            {
                score = minScore;
                if (!isInLevelEnd)
                    ScoreIsZero();
                else if (isInLevelEnd)
                    StopShooting();

            }

            SendScoreData();

            
            if(endLevelCanFire)
            ChooseAnimation();
        }
        else
        {
            handAim.weight = 0;
            playerAnimator.SetTrigger("Idle");
        }
        
       
    }

    void SendScoreData()
    {
        foreach(Scaler scaler in bodyParts)
        {
            scaler.score = score;
        }
    }

    void IncreaseScore(float extraScore)
    {
        score += extraScore;
    }

    void SyncHeight(int index)
    {
        if (index == 0)
        {
            currentHeight = footTip.position.y;
            heightDifference = currentHeight - initialTipHeight;
        }
        else if (index == 1)
        {
            currentHeight = hipPoint.position.y;
            heightDifference = currentHeight - initialTipHeight;
            heightDifference -= 0.3f;
        }
        else if(index == 2)
        {
            currentHeight = hipPoint.position.y;
            heightDifference = currentHeight - initialTipHeight;
            heightDifference += 0.7f;
        }

        transform.position = new Vector3(transform.position.x, transform.position.y - heightDifference, transform.position.z);
    }

    void ChooseAnimation()
    {
        if (score <= 0)
        {

            skin.SetBlendShapeWeight(0, 0);

            if (score >= runToCrawlTh)
            {
                SyncHeight(0);
                handAim.weight = 100;

                CheckAnimation(0);
                
                
                pSpline.SetSpeed(runSpeed);
                if(!isFalling)
                RunningFire();
            }
            else if (score >= CrawlToHopTh)
            {
                SyncHeight(1);
                handAim.weight = 0;
                CheckAnimation(1);
               
                pSpline.SetSpeed(crawlSpeed);
                if(!isFalling)
                CrawlAndHopFire();
            }
            else
            {
                SyncHeight(1);
                CheckAnimation(2);
                handAim.weight = 0;


                pSpline.SetSpeed(hopSpeed);
                if(!isFalling)
                CrawlAndHopFire();

            }
        }
        else
        {
            skin.SetBlendShapeWeight(0, score);
            
            handAim.weight = 100;
            CheckAnimation(0);
           
            
            pSpline.SetSpeed(runSpeed);
            if(!isFalling)
            RunningFire();
        }
    }

    void RunningFire()
    {
        timeSinceFired += Time.deltaTime;
        if(timeSinceFired >= delayBetweenBullets)
        {
            canFire = true;
        }
        if (canFire)
        {
            FireToPath(leftBulletPath);
            FireToPath(rightBulletPath);
            
            
            timeSinceFired = 0;
            canFire = false;
        }

    }

    void CrawlAndHopFire()
    {
        timeSinceFired += Time.deltaTime;
        if (timeSinceFired >= delayBetweenBullets)
        {
            canFire = true;
        }
        if (canFire)
        {
            //HeadFire();
            FireToPath(headBulletPath);
            timeSinceFired = 0;
            canFire = false;
        }
    }

    void TakeDamage()
    {
        float scorePlaceholder = score - bulletDamage;
        if(scorePlaceholder >= minScore)
        score = scorePlaceholder;
        else
        {
            score = minScore;
        }
        //bloodFXGO.SetActive(true);
        //Invoke(nameof(HideBlood), BLOOD_FX_LIFETIME);
    }

    void HideBlood()
    {
        //bloodFXGO.SetActive(false);
    }

    //void HeadFire()
    //{
    //    GameObject newBulletGO = Instantiate(bullet, hipTransform.position, Quaternion.identity);
    //    Material bulletMat = newBulletGO.GetComponent<Renderer>().material;
    //    bulletMat = playerMat;
    //    newBulletGO.GetComponent<Renderer>().material = bulletMat;
    //    BulletManager newBullet = newBulletGO.GetComponent<BulletManager>();
    //    newBullet.Launch(hipTransform, headTransform);
    //}
    //void ArmFire()
    //{
       
    //        GameObject newBulletGO = Instantiate(bullet, leftBulletPath[0].position, Quaternion.identity);
    //        Material bulletMat = newBulletGO.GetComponent<Renderer>().material;
    //        bulletMat = playerMat;
    //        newBulletGO.GetComponent<Renderer>().material = bulletMat;
    //        BulletManager newBullet = newBulletGO.GetComponent<BulletManager>();
    //        newBullet.transform.parent = transform;
    //        newBullet.Launch(hipTransform, handTransform[i]);
        
       
    //}

    void FireToPath(Transform[] path)
    {
        GameObject newBulletGO = Instantiate(bullet, path[0].position, Quaternion.identity);
        Material bulletMat = newBulletGO.GetComponent<Renderer>().material;
        bulletMat = playerMat;
        newBulletGO.GetComponent<Renderer>().material = bulletMat;
        BulletManager newBullet = newBulletGO.GetComponent<BulletManager>();
        if (increseBulletTime)
        {
            newBullet.timeLimit = levelEndBulletTimeLimit;
        }
        newBullet.bulletParent = bulletParent;
        newBullet.transform.parent = transform;
        newBullet.Launch(path);
        if (shootAtEndTarget)
        {
            newBullet.shootAtEndTarget = true;
            newBullet.endTarget = endTarget;

        }
    }


    void OnWallHit()
    {
        //follower.follow = false;
        // double currentPercent = follower.result.percent;
        //follower.motion.applyPositionZ = false;

        //isFalling = true;

        //transform.DOJump(new Vector3(transform.position.x, transform.position.y, transform.position.z - 5), 0,1, fallbackAnimLength);

        //CheckAnimation(3);

        //Debug.Log("BackForce  applied");

        //timeSinceFired = 0;
        //Invoke(nameof(FollowBack), fallbackAnimLength);

        score -= wallDamage;
    }

    void FollowBack()
    {
        
        //double newPercent = pSpline.computer.Travel(0, transform.position.z, Spline.Direction.Forward);
        //follower.SetPercent(newPercent);
        SplineSample splineSample = new SplineSample();
        follower.Project(transform.position, splineSample);
        follower.SetPercent(splineSample.percent);
        follower.motion.applyPositionZ = true;
        isFalling = false;
        //follower.follow = true;
    }

    void CheckAnimation(int index)
    {
        //0 run 1 crawl 2 hop 3 fall 4 idle
        for(int i = 0; i < animations.Length; i++)
        {
            if(i != index)
            {
                animations[i] = false;
            }

        } 
        if(index == 0)
        {
            if (!animations[0])
            {
                playerAnimator.SetTrigger("Run");
                animations[0] = true;
            }
        }
        else if(index == 1)
        {
            if (!animations[1])
            {
                playerAnimator.SetTrigger("Crawl");
                animations[1] = true;
            }
        }
        else if(index == 2)
        {
            if (!animations[2])
            {
                playerAnimator.SetTrigger("Hop");
                animations[2] = true;
            }
        }
        
    }

    void StopShooting()
    {
        endLevelCanFire = false;
    }
}
