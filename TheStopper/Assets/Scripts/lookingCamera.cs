using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lookingCamera : MonoBehaviour
{
    [SerializeField] GameObject cameraObject;
    void LateUpdate()
    {
        transform.LookAt(cameraObject.transform);
    }
}
