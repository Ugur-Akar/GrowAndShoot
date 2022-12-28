using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class UIManager : MonoBehaviour
{

    // Settings
    public float defaultStartWaitTime = 0.2f;
    public  Action OnLevelStart, OnNextLevel, OnLevelRestart;

    [Header("Screens")]
    public GameObject startCanvas;
    public GameObject ingameCanvas;
    public GameObject finishCanvas;
    public GameObject failCanvas;
    [Header("In Game")]
    public LevelBarDisplay levelBarDisplay;
    public TextMeshProUGUI inGameScoreText;
    public TextMeshProUGUI endGameScoreText;
    [Header("Finish Screen")]
    public ScoreTextManager scoreText;
    
    
    void Start()
    {
        CheckAndDisplayStartScreen();
    }
    
    void CheckAndDisplayStartScreen()
    {
        int displayStart = PlayerPrefs.GetInt("displayStart", 1);
        if(displayStart > 0)
        {
            startCanvas.SetActive(true);
        }
        else
        { 
            StartLevel();
            Invoke(nameof(StartLevelButton), defaultStartWaitTime);
           // StartLevelButton();
            PlayerPrefs.SetInt("displayStart", 1);

        }
    }

    #region Handler Functions

    public void StartLevelButton()
    {
        OnLevelStart?.Invoke();
        
    }

    public void NextLevelButton()
    {
        PlayerPrefs.SetInt("displayStart", 0);
        OnNextLevel?.Invoke();

    }

    public void RestartLevelButton()
    {
        PlayerPrefs.SetInt("displayStart", 0);
        OnLevelRestart?.Invoke();
    }

    #endregion

    public void StartLevel()
    {
        startCanvas.SetActive(false);
        ingameCanvas.SetActive(true);
        
    }

    public void SetInGameScore(int score)
    {
        inGameScoreText.text = "" + score;
    }

    public void SetInGameScoreAsText(string scoreText)
    {
        inGameScoreText.text = scoreText;
    }


    public void DisplayScore(int score, int oldScore=0)
    {
        scoreText.DisplayScore(score, oldScore);
    }

    public void DisplayScoreAsText(string scoreText)
    {
        endGameScoreText.text = scoreText;
    }

    public void SetLevel(int level)
    {
        levelBarDisplay.SetLevel(level);
    }

    public void UpdateProgess(float progress)
    {
        levelBarDisplay.DisplayProgress(progress);
    }

    public void FinishLevel()
    {
        ingameCanvas.SetActive(false);
        finishCanvas.SetActive(true);
    }

    public void FailLevel()
    {
        ingameCanvas.SetActive(false);
        failCanvas.SetActive(true);
    }




    void InitStates()
    {
        ingameCanvas.SetActive(false);
        finishCanvas.SetActive(false);
        failCanvas.SetActive(false);
        startCanvas.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
