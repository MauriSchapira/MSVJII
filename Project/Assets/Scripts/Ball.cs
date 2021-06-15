using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody rb;
    public Vector3 lastFrameVelocity;
    private float minVelocity;
    private Vector3 lastKnownGoodPosition;
    [SerializeField] private int timesToDissapearBall;
    [SerializeField] private float timeToResetPosition;
    private Vector3 originalScale;
    private CameraTransitionManager camTransManager;
    [SerializeField] private float reappearGroundOffset;
    [Range(0, 1)] [SerializeField] private float shrinkerValue;
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
        lastFrameVelocity = Vector3.zero;
    }

    private void FixedUpdate()
    {
        lastFrameVelocity = rb.velocity;
    }

    private void OnCollisionStay(Collision collision)
    {
        Debug.Log($"{collision.GetContact(0).thisCollider.name}");
        rb.velocity *= 0.1f;
        //Bounce(collision.contacts[0].normal);
        if (lastFrameVelocity != Vector3.zero)
        {
            foreach (var item in collision.contacts)
            {
                Debug.Log($"{item.normal}");

                Debug.Log($"{lastFrameVelocity}in");
                Bounce(item.normal);
                Debug.Log($"{lastFrameVelocity}out");
            }
            rb.velocity = lastFrameVelocity;
        }
    }

    private void Bounce(Vector3 collisionNormal)
    {
        if (Vector3.Dot(lastFrameVelocity, collisionNormal) < 0)
        {
            var speed = lastFrameVelocity.magnitude;
            var direction = Vector3.Reflect(lastFrameVelocity.normalized, collisionNormal);
            lastFrameVelocity = direction * Mathf.Max(speed, minVelocity);
        }

    }

}
