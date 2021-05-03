using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private GameObject arrow;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = GameObject.FindGameObjectWithTag("BallStartPosition").transform.position;
        rb = GetComponent<Rigidbody>();
        rb.sleepThreshold = 0.5f;
        rb.maxAngularVelocity = Mathf.Infinity;      
    }

    private void Update()
    {
        arrow.transform.position = transform.position;
    }

}
