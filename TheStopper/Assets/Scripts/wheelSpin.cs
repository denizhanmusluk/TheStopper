using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wheelSpin : MonoBehaviour
{
    void Update()
    {

            transform.Rotate(200 * Time.deltaTime, 0, 0);
    }
}
