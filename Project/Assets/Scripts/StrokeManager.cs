using System.Net.WebSockets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrokeManager : MonoBehaviour
{
    public enum StartingAngle { ZForward = 0, XForward = 90, ZBackward = 180, XBackward = 270 };

    [Tooltip("A que direccion apunta al principio")]
    public StartingAngle startingAngle;
    public StrokeState StrokeMode { get; protected set; }

    public delegate void MaxStrokesReached();
    public MaxStrokesReached OnMaxStrokesReached;



    [SerializeField] private int maxStrokes;

    private Rigidbody playerBallRB;
    public Ball ball;

    private float strokeAngle;
    public float StrokeAngle
    {
        get
        {
            return strokeAngle;
        }
        protected set
        {
            strokeAngle = value;
            ui.ChangeArrowAngle(strokeAngle, Camera.main.transform.rotation.eulerAngles.y);
        }

    }

    /*[SerializeField] private GameObject hitArrow;*/

    public float StrokeForce { get; protected set; }
    public float StrokeForcePerc { get { return StrokeForce / (MaxStrokeForce * currentGolfClub.GeneralStrength); } }

    public int StrokeCount { get; protected set; }

    [SerializeField] private float angleChangeSpeed = 100;

    private float strokeForceFillSpeed = 5f;
    [SerializeField] private float strikeFillSpeed;



    [SerializeField] float MaxStrokeForce = 10f;

    [SerializeField] private UI ui;

    [SerializeField] private GolfClub[] golfClubsAvailable;
    private GolfClub currentGolfClub;
    private int golfClubIndex;

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
        StrokeAngle = (float)startingAngle;
        ChangeState(StrokeState.Aiming);
        golfClubIndex = -1;
        ChangeGolfClub();
        GameManager.instance.StrokeManagerRef(this);
    }

    private void FindPlayerBall()
    {

        GameObject go = GameObject.FindGameObjectWithTag("Player");
        ball = go.GetComponent<Ball>();
        playerBallRB = go.GetComponent<Rigidbody>();
    }

    private void ChangeGolfClub()
    {
        golfClubIndex++;

        if (golfClubIndex >= golfClubsAvailable.Length)
        {
            golfClubIndex = 0;
        }

        currentGolfClub = golfClubsAvailable[golfClubIndex];

        UpdateGolfClubUI();

    }

    private void UpdateGolfClubUI()
    {
        ui.UpdateGolfClub(currentGolfClub.ClubSprite, currentGolfClub.ClubName);
    }


    private void Update()
    {
        switch (StrokeMode)
        {
            case StrokeState.Aiming:

                if (StrokeCount >= maxStrokes)
                {
                    OnMaxStrokesReached?.Invoke();
                }

                StrokeAngle += Input.GetAxis("Horizontal") * angleChangeSpeed * Time.deltaTime;

                if (Input.GetKeyDown(KeyCode.Tab))
                {
                    ChangeGolfClub();
                }

                if (Input.GetButtonUp("Fire1"))
                {
                    ChangeState(StrokeState.ForceSet);
                }

                break;

            case StrokeState.ForceSet:

                StrokeForce += (strokeForceFillSpeed * Input.mouseScrollDelta.y * strikeFillSpeed) * Time.deltaTime * currentGolfClub.GeneralStrength;

                if (StrokeForce > MaxStrokeForce * currentGolfClub.GeneralStrength)
                {
                    StrokeForce = MaxStrokeForce * currentGolfClub.GeneralStrength;
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
        if (playerBallRB.IsSleeping())
        {
            ball.SetKnownGoodPosition(ball.transform.position);
            ChangeState(StrokeState.Aiming);
        }
    }

    private void HitBall()
    {
        Vector3 forceVec = StrokeForce * Vector3.forward + StrokeForce * Vector3.up * currentGolfClub.VerticalFactorStrength;
        playerBallRB.AddForce(Quaternion.Euler(0, StrokeAngle, 0) * forceVec, ForceMode.Impulse);
        ball.lastFrameVelocity = Quaternion.Euler(0, StrokeAngle, 0) * forceVec;
        StrokeForce = 0;
        StrokeCount++;
        ui.UpdateStrokes(StrokeCount);
        ChangeState(StrokeState.Move);
    }

    private void EnableArrow(bool isEnabled)
    {
        //hitArrow.SetActive(isEnabled);
        ui.EnableDisableArrow(isEnabled);
    }



    private void ChangeState(StrokeState newState)
    {
        StrokeMode = newState;

        switch (StrokeMode)
        {
            case StrokeState.Aiming:
                ui.EnableDisableFillImage(false);
                ui.EnableDisableGolfClub(true);
                EnableArrow(true);
                break;

            case StrokeState.ForceSet:
                ui.EnableDisableFillImage(true);
                break;

            case StrokeState.Hit:
                EnableArrow(false);
                ui.EnableDisableFillImage(false);
                ui.EnableDisableGolfClub(false);
                HitBall();
                break;

            default:
                break;
        }
    }
}
