using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wheelSpin : MonoBehaviour
{
    void Update()
    {
        if (!Globals.finish)
        {
            transform.Rotate(200 * Time.deltaTime, 0, 0);
        }
    }
}
