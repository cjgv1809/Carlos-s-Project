using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float cameraSpeed = 0.025f;
    public Vector3 movement;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 desiredPosition = target.position + movement;
        Vector3 smoothPosition = Vector3.Lerp(transform.position, desiredPosition, cameraSpeed);
        transform.position = smoothPosition;
    }
}
