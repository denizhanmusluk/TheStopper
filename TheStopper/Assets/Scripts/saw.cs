using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class saw : MonoBehaviour
{
    [SerializeField] Transform firstPoint, lastPoint;
    [SerializeField] Transform moveObj;
    Transform targ;
    [SerializeField] float speed;
    void Start()
    {
        targ = lastPoint;
    }

    void Update()
    {
        if (moveObj != null)
        {
            if (Vector3.Distance(moveObj.position, firstPoint.position) < 1f)
            {
                targ = lastPoint;
            }

            if (Vector3.Distance(moveObj.position, lastPoint.position) < 1f)
            {
                targ = firstPoint;
            }
            moveTarget();
        }
    }
    void moveTarget()
    {
        moveObj.position = Vector3.Lerp(moveObj.position, targ.position, speed * Time.deltaTime);
    }
}
