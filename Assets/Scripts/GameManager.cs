using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;
using UnityEngine.UI;

/// <summary> Manages the state of the whole application </summary>
public class GameManager : Singleton<GameManager>
{
    [SerializeField] private string gameScene;
    [SerializeField] private Text highScore;
    //private string path = "";
    //private string persistentPath = "";

    private void Start()
    {
        SetPaths();
        LoadData();
        
    }

    public void Play()
    {
        StartCoroutine(LoadScene(gameScene));
    }

    private IEnumerator LoadScene(string sceneName)
    {
        Debug.Log("Loading game!");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(sceneName);
        //EditorSceneManager.LoadScene(sceneName);
    }

    private void SetPaths()
    {
       // path = Application.dataPath + Path.AltDirectorySeparatorChar + Utility.FIleName;
        Utility.persistentPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar + Utility.FIleName;
    }

    public void LoadData()
    {
        if (Utility.HighScore == 0)
        {
            if (File.Exists(Utility.persistentPath))
            {

                string json = File.ReadAllText(Utility.persistentPath);
                Debug.Log("HighScore: " + json);
                HighScore data = JsonUtility.FromJson<HighScore>(json);
                Utility.HighScore = data.score;
                Debug.Log(data.ToString());
            }
            else
            {
                Utility.HighScore = 0;
            }
        }
        
        setHighScore();
    }
    void setHighScore()
    {
        highScore.text = "Highscore: " + Utility.HighScore;
    }
}