using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
using UnityEngine.SceneManagement;
using ElephantSDK;

public class GameManager : MonoBehaviour
{
    //Settings
    public float enemyBulletTimeLimit;
    public float minScore;
    public float maxScore;
    public float endGameDiminishRate = 20;
    // Connections
    public GameObject player;
    public SplineComputer currentRoad;
    public List<GameObject> levelList;
    public PlayerManager playerManager;
    GateManager gateManager;
    PlayerSplineControl playerSplineControl;
    LevelManager currentLevelManager;
    public GameObject currentLevelPrefab;
    public UIManager uiManager;
    GameObject currentLevelGO;
    public Transform bulletParent;

    SplineFollower playerFollower;
    // State Variables
    int levelIndex;
    bool levelEndFirstEntry;
    int nRemovedCubes;
    bool finishUiShown;
    public int points;
    
    // Start is called before the first frame update
    private void Awake()
    {
        levelIndex = PlayerPrefs.GetInt("levelIndex",0);
        
        currentLevelPrefab = levelList[levelIndex%levelList.Count];

        playerManager.minScore = minScore;
        playerManager.maxScore = maxScore;
        AwakeConnections();

    }

    void AwakeConnections()
    {
        playerFollower = player.GetComponent<SplineFollower>();
        playerSplineControl = player.GetComponent<PlayerSplineControl>();

        playerManager.ScoreIsZero += FailLevel;
        playerSplineControl.LevelEnd += LevelEndStart;
        uiManager.OnLevelStart += OnLevelStart;
        uiManager.OnLevelRestart += OnLevelRestart;
        uiManager.OnNextLevel += OnNextLevel;
        
    }

    void Start()
    {
        InitConnections();
        InitState();

        points = 0;

        InstantiateLevel();
    }
    void InitConnections(){
        
    }
    void InitState(){
        levelEndFirstEntry = true;
        nRemovedCubes = 0;
        finishUiShown = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!levelEndFirstEntry)
        {
            if (playerManager.score <= minScore && !finishUiShown)
            {
                Elephant.LevelCompleted(levelIndex);
                levelIndex++;
                DestroyBulletsOnScene();
                
                PlayerPrefs.SetInt("levelIndex", levelIndex);
                uiManager.DisplayScoreAsText("x" + nRemovedCubes);

                Invoke(nameof(DestroyBulletsOnScene), 1);
                uiManager.FinishLevel();
                uiManager.scoreText.gameObject.SetActive(true);
                uiManager.DisplayScore(points*nRemovedCubes*10, 0);
                playerManager.isInLevel = false;
                finishUiShown = true;


            }
        }
    }

    void InstantiateLevel()
    {

        currentLevelGO = Instantiate(currentLevelPrefab, transform.position, Quaternion.identity);
        currentLevelManager = currentLevelGO.GetComponent<LevelManager>();

        InitLevel();
    }

    void InitLevel()
    {
        currentLevelManager.AddEnemiesToList();

        for(int i = 0; i < currentLevelManager.enemies.Count; i++)
        {
            currentLevelManager.enemies[i].player = player.transform;
            currentLevelManager.enemies[i].timeLimit = 0;
            currentLevelManager.enemies[i].IncreasePoints += IncreasePoints;
            if(currentLevelManager.enemies[i].player == null)
            {
                Debug.Log("PlayerTransform Failed!"); 
            }
        }
        currentLevelManager.AssignPlayer(player);
        playerManager.endTarget = currentLevelManager.endTarget; 
        currentRoad = currentLevelManager.splineComputer;
        playerSplineControl.computer = currentRoad;
        playerFollower.spline = currentRoad;
        playerFollower.SetPercent(0);
        gateManager = currentLevelManager.gateManager;
        Elephant.LevelStarted(levelIndex);
    }

    void OnLevelStart()
    { 
        playerSplineControl.StartMoving();
        currentLevelManager.OnCubeRemoved += OnCubeRemoved;
        
        for(int i = 0; i < currentLevelManager.enemies.Count; i++)
        {
            currentLevelManager.enemies[i].SetInLevel();
            currentLevelManager.enemies[i].timeLimit = enemyBulletTimeLimit;
        }

        playerManager.isInLevel = true;
        playerManager.score = 0;
        
    }

    void OnLevelRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, 0);
    }

    void OnNextLevel()
    {               
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, 0);
    }

    void OnCubeRemoved()
    {
        nRemovedCubes++;
    }

    void LevelEndStart()
    {   
        levelEndFirstEntry = false;
        playerManager.shootAtEndTarget = true;
        DestroyBulletsOnScene();

        playerManager.diminishRate = 20;
        playerManager.delayBetweenBullets /= 2;
        playerManager.increseBulletTime = true;
        gateManager.OpenSesame();
        
        playerSplineControl.StopMoving();
        playerFollower.motion.offset = new Vector2(0, playerFollower.motion.offset.y);
        playerManager.isInLevelEnd = true;
        currentLevelManager.UnlockEndGame();
        
        

    }

    void FailLevel()
    {
        Debug.Log("LevelFailed");
        playerFollower.follow = false;
        playerManager.isInLevel = false;
        uiManager.FailLevel();
        Elephant.LevelFailed(levelIndex);
    }

    void DestroyBulletsOnScene()
    {
        for (int i = 0; i < bulletParent.childCount; i++)
        {
            Destroy(bulletParent.GetChild(i).gameObject);
        }
    }

    void IncreasePoints()
    {
        points++;
        Debug.Log(points);
    }
}
