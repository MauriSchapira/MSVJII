using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public delegate void SetGoodPosition(int camera, Vector3 position);
    public delegate void ReturnToGoodPosition(int camera, Vector3 position);

    public LevelManager lvlManager;
    public StrokeManager strokeManager;
    public Goal goal;

   

    public SetGoodPosition OnSetGoodPosition;
    public ReturnToGoodPosition OnReturnToGoodPosition;


    private void Awake()
    {
        MakeSingleton();
    }
    private void Start()
    {
        lvlManager.OnNextLevel += OnNextLevelHandler;
    }
    public void StrokeManagerRef(StrokeManager strokeManagerReference)
    {
        strokeManager = strokeManagerReference;
        strokeManager.OnMaxStrokesReached += OnMaxStrokesReachedHandler;
    }

    private void OnMaxStrokesReachedHandler()
    {
        lvlManager.OnWonLevelHandler();
    }
    public void GoalReference (Goal goal)
    {
        this.goal = goal;
        goal.OnWin += lvlManager.OnWonLevelHandler;
    }

    private void OnNextLevelHandler(string levelToLoad)
    {
        goal.OnWin -= lvlManager.OnWonLevelHandler;
        LoadScene(levelToLoad);
    }

    public void ReloadScene()
    {
        LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadScene(string sceneToLoad)
    {
        SceneManager.LoadSceneAsync(sceneToLoad);
    }
    

    private void MakeSingleton()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);           

        }

        else
        {
            Destroy(gameObject);
        }
    }
}
