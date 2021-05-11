using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;




    private void Awake()
    {
        MakeSingleton();
    }


    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
