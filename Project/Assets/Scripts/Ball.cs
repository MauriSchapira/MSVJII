using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private GameObject arrow;
    private Vector3 lastKnownGoodPosition;
    [SerializeField] private int timesToDissapearBall;
    [SerializeField] private float timeToResetPosition;
    private Vector3 originalScale;
    [SerializeField] private float maxDistance;
    private GameObject hole;


    // Start is called before the first frame update
    void Start()
    {
        hole = GameObject.FindGameObjectWithTag("Hole");
        originalScale = transform.localScale;
        transform.position = GameObject.FindGameObjectWithTag("BallStartPosition").transform.position;
        rb = GetComponent<Rigidbody>();
        rb.sleepThreshold = 0.5f;
        rb.maxAngularVelocity = Mathf.Infinity;      
    }

    
    public void SetKnownGoodPosition(Vector3 pos)
    {
        lastKnownGoodPosition = pos;
    }

    public void ToKnownGoodPosition()
    {
        StartCoroutine(GoToKnownGoodPosition());
    }

    private IEnumerator GoToKnownGoodPosition()
    {

        for (int i = 0; i < timesToDissapearBall; i++)
        {
            transform.localScale *= (timesToDissapearBall - i) / (timesToDissapearBall);
            yield return new WaitForSecondsRealtime(timeToResetPosition);
        }

        transform.position = lastKnownGoodPosition;
        transform.localScale = originalScale;
    }

    private void CheckHoleDistance()
    {      
        if (Vector3.Distance (transform.position,hole.transform.position) >= maxDistance)
        {
            ToKnownGoodPosition();
        }
    }


    private void Update()
    {
        arrow.transform.position = transform.position;

        CheckHoleDistance();
    }

}
