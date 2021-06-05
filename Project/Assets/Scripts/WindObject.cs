using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindObject : MonoBehaviour
{

    [SerializeField] private bool isBlowing;
    public Vector3 WindDirection
    {
        get
        {
            return windDirectionTester;
        }
        set
        {
            WindDirection = value;
        }
    }

    [SerializeField] private ParticleSystem particleSys;
    public float WindForce => windForce;

    [SerializeField] private float windForceToUpDirection;

    [SerializeField] private Vector3 windDirectionTester;
    [SerializeField] private float windForce;

    public void StopOrStartBlowing(bool newState)
    {
        isBlowing = newState;
    }

    private void Start()
    {
        //print(WindDirection + " is wind direction");
    }


    //Wind from XZ plane
    private void OnTriggerStay(Collider other)
    {
        if (!isBlowing) return;

        other.attachedRigidbody.AddForce(WindDirection.x * transform.right * windForce + WindDirection.z * transform.forward * windForce, ForceMode.Force);
    }


    //Separate impulse code
    private void OnTriggerEnter(Collider other)
    {
        if (!isBlowing) return;
        other.attachedRigidbody.AddForce(WindDirection.y * Vector3.up * windForceToUpDirection, ForceMode.Impulse);
    }
}
