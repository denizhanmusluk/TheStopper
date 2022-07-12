using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class headLight : MonoBehaviour
{
 
    void Update()
    {
        transform.Rotate(0, 500 * Time.deltaTime, 0);
    }
}
