using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrokeManager : MonoBehaviour
{
    
    public StrokeState StrokeMode { get; protected set; }

    private Rigidbody playerBallRB;
    public float StrokeAngle { get; protected set; }

    [SerializeField] private GameObject hitArrow;

    public float StrokeForce { get; protected set; }
    public float StrokeForcePerc { get { return StrokeForce / MaxStrokeForce; } }

    public int StrokeCount { get; protected set; }

    [SerializeField] private float angleChangeSpeed = 100;

    private float strokeForceFillSpeed = 5f;
    [SerializeField] private float strikeFillSpeed;

    private int fillDir = 1;

    float MaxStrokeForce = 10f;

    [SerializeField] private UI ui;

    public enum StrokeState 
    {
        Aiming,
        ForceSet,
        Hit,
        Move
    };

    void Start()
    {
        FindPlayerBall();
        StrokeCount = 0;
        ChangeState(StrokeState.Aiming);    

    }

    private void FindPlayerBall()
    {
        GameObject go = GameObject.FindGameObjectWithTag("Player");
        playerBallRB = go.GetComponent<Rigidbody>();    
    }

  
    private void Update()
    {
        switch (StrokeMode)
        {
            case StrokeState.Aiming:

                StrokeAngle += Input.GetAxis("Horizontal") * angleChangeSpeed * Time.deltaTime;

                if (Input.GetButtonUp("Fire1"))
                {
                    ChangeState(StrokeState.ForceSet);
                }

                break;

            case StrokeState.ForceSet:

                StrokeForce += (strokeForceFillSpeed * Input.mouseScrollDelta.y * strikeFillSpeed) * Time.deltaTime;                

                if (StrokeForce > MaxStrokeForce)
                {
                    StrokeForce = MaxStrokeForce;                   
                }
                else if (StrokeForce < 0)
                {
                    StrokeForce = 0;
                }

                ui.ChangeFillImageFill(StrokeForcePerc);

                if (Input.GetMouseButtonDown(1))
                {
                    ChangeState(StrokeState.Aiming);
                }

                if (Input.GetButtonUp("Fire1"))
                {
                    ChangeState(StrokeState.Hit);                    
                }

                break;
            case StrokeState.Hit:

                break;

            case StrokeState.Move:
                CheckRollingStatus();

                break;
            default:
                break;
        }     
    }

    void CheckRollingStatus()
    {
        if(playerBallRB.IsSleeping())
        {
            ChangeState(StrokeState.Aiming);
        }
    }

    private void HitBall()
    {
        Vector3 forceVec = StrokeForce * Vector3.forward;
        playerBallRB.AddForce(Quaternion.Euler(0, StrokeAngle, 0) * forceVec, ForceMode.Impulse);
        StrokeForce = 0;
        fillDir = 1;
        StrokeCount++;
        ui.UpdateStrokes(StrokeCount);
        ChangeState(StrokeState.Move);
    }

    private void EnableArrow(bool isEnabled)
    {
        hitArrow.SetActive(isEnabled);
    }

    

    private void ChangeState(StrokeState newState)
    {
        StrokeMode = newState;

        switch (StrokeMode)
        {
            case StrokeState.Aiming:
                ui.EnableDisableFillImage(false);
                EnableArrow(true);
                break;

            case StrokeState.ForceSet:
                ui.EnableDisableFillImage(true);
                break;

            case StrokeState.Hit:
                EnableArrow(false);
                ui.EnableDisableFillImage(false);
                HitBall();
                break;
           
            default:
                break;
        }
    }
}
