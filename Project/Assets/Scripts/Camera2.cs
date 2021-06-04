using UnityEngine;
using System.Collections;
 
[AddComponentMenu("Camera-Control/Mouse Orbit with zoom")]
public class Camera2 : MonoBehaviour {
 
    public Transform target;
    public float distance = 5.0f;
    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;
 
    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;
 
    public float distanceMin = .5f;
    public float distanceMax = 15f;
 
    private Rigidbody rigidbody;
    private Vector3 _cameraOffset;
    [Tooltip("The smooth factor when the camera follows a target object")]
    [Range(0.2f, 1f)]
    public float cameraFollowSmoothness;
 
    float x = 0.0f;
    float y = 0.0f;
 
    // Use this for initialization
    void Start () 
    {
        _cameraOffset = transform.position - target.position;

        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
 
        rigidbody = GetComponent<Rigidbody>();
 
        // Make the rigid body not change rotation
        if (rigidbody != null)
        {
            rigidbody.freezeRotation = true;
        }
    }
 
    void LateUpdate () 
    {
        // If target
        if (Input.GetMouseButton(1)) 
        {
            x += Input.GetAxis("Mouse X") * xSpeed * distance * 0.02f;
            y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
 
            y = ClampAngle(y, yMinLimit, yMaxLimit);
 
            Quaternion rotation = Quaternion.Euler(y, x, 0);
 
            distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel")*5, distanceMin, distanceMax);
 
            RaycastHit hit;
            if (Physics.Linecast (target.position, transform.position, out hit)) 
            {
                distance -=  hit.distance;
            }
            Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
            Vector3 position = rotation * negDistance + target.position;
 
            transform.rotation = rotation;
            transform.position = position;
            _cameraOffset = transform.position - target.position;
        }

        if (!Input.GetMouseButton(1))
        {
            FollowTarget();
            transform.LookAt(target);
        }
    }
 
    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }

    private void FollowTarget()
    {
        var newPosition = target.position + _cameraOffset;
        transform.position = Vector3.Slerp(transform.position, newPosition, cameraFollowSmoothness);
    }
}