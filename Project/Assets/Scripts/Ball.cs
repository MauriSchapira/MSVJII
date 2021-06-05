using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody rb;
    
    private Vector3 lastKnownGoodPosition;
    [SerializeField] private int timesToDissapearBall;
    [SerializeField] private float timeToResetPosition;
    private Vector3 originalScale;
    private CameraTransitionManager camTransManager;
    [SerializeField] private float reappearGroundOffset;
    [Range (0,1)] [SerializeField] private float shrinkerValue;
    private bool dissapearing;
    public bool Dissapearing
    {
        get
        {
            return dissapearing;
        }
        private set
        {
            dissapearing = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
      
        originalScale = transform.localScale;
        GameObject ballStartPos = GameObject.FindGameObjectWithTag("BallStartPosition");
        transform.position = ballStartPos.transform.position;
        Destroy(ballStartPos);       
        rb = GetComponent<Rigidbody>();
        rb.sleepThreshold = 0.5f;
        rb.maxAngularVelocity = Mathf.Infinity;
        camTransManager = FindObjectOfType<CameraTransitionManager>();
        SetKnownGoodPosition(transform.position);
        
    }

    
    public void SetKnownGoodPosition(Vector3 pos)
    {
        lastKnownGoodPosition = pos;
        //camTransManager.ChangeLastKnownGoodCamera();

    }

    public void ToKnownGoodPosition()
    {
        Dissapearing = true;
        StartCoroutine(GoToKnownGoodPosition());
    }

    private IEnumerator GoToKnownGoodPosition()
    {    
        
        for (int i = 0; i < timesToDissapearBall; i++)
        {
            transform.localScale *= shrinkerValue;
            yield return new WaitForSecondsRealtime(timeToResetPosition);
        }

        ResetVelocities();
        transform.position = lastKnownGoodPosition + Vector3.up * reappearGroundOffset;        
        transform.localScale = originalScale;
        //camTransManager.ToLastKnownGoodCamera();
        Dissapearing = false;
    }

  private void ResetVelocities()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}
