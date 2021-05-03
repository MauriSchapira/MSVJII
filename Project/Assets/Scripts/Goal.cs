using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{

    [SerializeField] private string sceneToReload;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(Win());
        }    
    }

    private IEnumerator Win()
    {
        print("You win!!");

        yield return new WaitForSecondsRealtime(2);

        SceneManager.LoadScene(sceneToReload);
    }



}
