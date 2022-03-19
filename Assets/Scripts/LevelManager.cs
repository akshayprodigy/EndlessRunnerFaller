using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary> Manages the state of the level </summary>
public class LevelManager : Singleton<LevelManager>
{
    public int Score { get; private set; }
    [SerializeField] private float levelInterval = 5f;
    [SerializeField]
    private GameObject player;

    [SerializeField] private string menuScene;

    private WaitForSeconds intervalWaitingPeroid;
    bool gameOver;
    public delegate void IncrementSpeed();
    public static event IncrementSpeed onIncrementSpeed;

    public delegate void UpdateScoreScore(int score);
    public static event UpdateScoreScore onUpdateScoreScore;

    public delegate void CurrentGameOver(int score, int highscore);
    public static event CurrentGameOver onCurrentGameOver;
    public delegate void CurrentGamePause(int score, int highscore);
    public static event CurrentGamePause onCurrentGamePause;
    public delegate void ShowCong();
    public static event ShowCong onShowCong;

    public bool showCongMsg = false;
    private void OnEnable()
    {
        MainCharacter.onPlayerScore += IncrementScore;
        MainCharacter.onPlayerOutOfBound += GameOver;
        UIManager.onResetLevel += ResetLevel;
        MainCharacter.onPauseGame += GamePause;
        UIManager.onContinueLevel += ContinueGameLevel;
        UIManager.onLoadMainMenu += LoadMenuScene;
    }

    private void OnDisable()
    {
        MainCharacter.onPlayerScore -= IncrementScore;
        MainCharacter.onPlayerOutOfBound -= GameOver;
        UIManager.onResetLevel -= ResetLevel;
        UIManager.onContinueLevel -= ContinueGameLevel;
        MainCharacter.onPauseGame -= GamePause;
        UIManager.onLoadMainMenu -= LoadMenuScene;
    }

    void Start()
    {
        gameOver = false;
        showCongMsg = false;
        intervalWaitingPeroid = new WaitForSeconds(levelInterval);
        StartCoroutine(PlatformController());
    }

    public void IncrementScore()
    {
        Score++;
        if(Score > Utility.HighScore)
        {
            Utility.HighScore = Score;

            // do some ui event
            if (!showCongMsg)
            {
                showCongMsg = true;
                if (onShowCong != null)
                    onShowCong();
            }

        }
        //UIManager.Instance.UpdateScore(Score);
        if (onUpdateScoreScore != null)
            onUpdateScoreScore(Score);
    }

    public void ResetLevel()
    {
        Reset();
        player.SetActive(true);
        player.GetComponent<MainCharacter>().ResetPlayer();
    }

    public void ContinueGameLevel()
    {
        Time.timeScale = 1;
    }

    public void GamePause()
    {
        Time.timeScale = 0;
        if (onCurrentGamePause != null)
            onCurrentGamePause(Score, Utility.HighScore);
    }

    public void Reset()
    {
        Score = 0;
        showCongMsg = false;
        //UIManager.Instance.UpdateScore(Score);
        if (onUpdateScoreScore != null)
            onUpdateScoreScore(Score);
        // reset logic
    }


    void GameOver()
    {
        //Time.timeScale = 0;
        gameOver = true;
        if (onCurrentGameOver != null)
            onCurrentGameOver(Score,Utility.HighScore);

        SaveData();
    }

    void LoadMenuScene()
    {
        if (Time.timeScale == 0)
            Time.timeScale = 1;
        SceneManager.LoadScene(menuScene);
        
    }

    

    IEnumerator PlatformController()
    {
        while (!gameOver)
        {
            yield return intervalWaitingPeroid;
            if (onIncrementSpeed != null)
                onIncrementSpeed();
        }
    }

    public void SaveData()
    {
        //string savePath = Utility.persistentPath;

        Debug.Log("Saving Data at " + Utility.persistentPath);
        HighScore playerData = new HighScore(Utility.HighScore);
        string json = JsonUtility.ToJson(playerData);
        Debug.Log(json);

        File.WriteAllText(Utility.persistentPath, json);
    }


    private void OnDestroy()
    {
        //SaveData();
    }
}
