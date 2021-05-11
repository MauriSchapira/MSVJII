using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public delegate void SetGoodPosition(int camera, Vector3 position);
    public delegate void ReturnToGoodPosition(int camera, Vector3 position);

    public SetGoodPosition OnSetGoodPosition;
    public ReturnToGoodPosition OnReturnToGoodPosition;


    private void Awake()
    {
        MakeSingleton();
    }


    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void CallSetGoodPosition()
    {

    }

    public void CallReturnToGoodPosition()
    {

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
