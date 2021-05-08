using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{

    [SerializeField] private string sceneToReload;
    [SerializeField] private float timeToWinStart;
    private float timeToWin;

    private void Start()
    {
        ResetTimer();
    }

    private void ResetTimer()
    {
        timeToWin = timeToWinStart;
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            timeToWin -= Time.deltaTime;

            if (timeToWin <= 0)
            {
                StartCoroutine(Win());
            }
        }    
    }

    private void OnTriggerExit(Collider other)
    {
        ResetTimer();
    }


    private IEnumerator Win()
    {
        print("You win!!");

        yield return new WaitForSecondsRealtime(2);

        SceneManager.LoadScene(sceneToReload);
    }



}
