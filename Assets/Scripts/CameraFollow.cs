using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    [SerializeField] private Transform target; //target's 3D transform
    [SerializeField] private Vector3 velocity = Vector3.zero; //reference variable for smoothdamp of camera position
    [SerializeField] private Vector3 offset; //offset for camera
    [SerializeField] private float smoothTime = 0.3f; //speed for linear movement


    void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void FixedUpdate()
    {
        Vector3 targetPosition = target.position + offset; //set up initial position with offset
        Vector3 smoothPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime); //move from initial position to target's position with linear speed
        
        smoothPosition.z = transform.position.z; //since this is 2D don't change the z axis
        transform.position = smoothPosition; //update position
    }
}
