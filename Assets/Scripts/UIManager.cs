using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject playerScore;
    Text playerScoreTxt;

    [SerializeField] GameObject gameOverUI;
    [SerializeField] GameObject gameOverHighiScore;
    [SerializeField] GameObject gameOverCurrentScore;
    Text gameOverHighiScoreTxt, gameOverCurrentScoreTxt;

    [SerializeField] GameObject gamePauseUI;
    [SerializeField] GameObject gamePauseHighiScore;
    [SerializeField] GameObject gamePauseCurrentScore;
    [SerializeField] float congartulationMsgDuration = 0.5f;
    Text gamePauseHighiScoreTxt, gamePauseCurrentScoreTxt;


    [SerializeField] GameObject CongratulationsMsg;

    public delegate void ResetLevel();
    public static event ResetLevel onResetLevel;

    public delegate void ContinueLevel();
    public static event ContinueLevel onContinueLevel;

    public delegate void LoadMainMenu();
    public static event LoadMainMenu onLoadMainMenu;

    private void OnEnable()
    {
        LevelManager.onUpdateScoreScore += UpdateScore;
        LevelManager.onCurrentGameOver += OnGameOver;
        PlatformManager.onPlatforReadyAfterReset += DeactivateGameOverMenu;
        LevelManager.onCurrentGamePause += OnGamePause;
        LevelManager.onShowCong += ShowCongratulations;
    }

    private void OnDisable()
    {
        LevelManager.onUpdateScoreScore -= UpdateScore;
        LevelManager.onCurrentGameOver -= OnGameOver;
        PlatformManager.onPlatforReadyAfterReset -= DeactivateGameOverMenu;
        LevelManager.onCurrentGamePause += OnGamePause;
        LevelManager.onShowCong -= ShowCongratulations;
    }

    private void Start()
    {
        playerScoreTxt = playerScore.GetComponent<Text>();

        gameOverHighiScoreTxt = gameOverHighiScore.GetComponent<Text>();
        gameOverCurrentScoreTxt = gameOverCurrentScore.GetComponent<Text>();

        gamePauseHighiScoreTxt = gamePauseHighiScore.GetComponent<Text>();
        gamePauseCurrentScoreTxt = gamePauseCurrentScore.GetComponent<Text>();
        DisableCongratulations();
        UpdateScore(0);
        StartrGamePlay();
    }

    void StartrGamePlay()
    {
        gameOverUI.SetActive(false);
        gamePauseUI.SetActive(false);
    }

    void OnGamePause(int score, int highscore)
    {
        gamePauseHighiScoreTxt.text = highscore.ToString();
        gamePauseCurrentScoreTxt.text = score.ToString();

        gamePauseUI.SetActive(true);
    }

    void OnGameOver(int score, int highscore)
    {
        gameOverHighiScoreTxt.text = highscore.ToString();
        gameOverCurrentScoreTxt.text = score.ToString();

        gameOverUI.SetActive(true);
    }

    void DeactivateGameOverMenu()
    {
        gameOverUI.SetActive(false);
    }

    public void onRestartLevel()
    {
        if (onResetLevel != null)
        {
            onResetLevel();
        }
    }

    public void onMainMenuClick()
    {
        if (onLoadMainMenu != null)
            onLoadMainMenu();
    }


    public void onContinueClick()
    {
        DeactivateGamePauseMenu();
        if (onContinueLevel != null)
            onContinueLevel();
    }

    void DeactivateGamePauseMenu()
    {
        gamePauseUI.SetActive(false);
    }


    void PlayerScore()
    {
        UpdateScore(LevelManager.Instance.Score);
    }

    public void UpdateScore(int score)
    {
        //Debug.Log("UpdateScore: " + score);
        playerScoreTxt.text = "Sore: "+score;
    }

    public void ShowCongratulations()
    {
        CongratulationsMsg.SetActive(true);
        Invoke("DisableCongratulations", congartulationMsgDuration);
    }

    public void DisableCongratulations()
    {
        CongratulationsMsg.SetActive(false);
    }

}
