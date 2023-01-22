using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obs : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<vehicle>() != null || other.GetComponent<SwipeControl>() != null)
        {
            transform.parent.parent.GetComponent<Obstacle>().triggerFunction(other);
            for (int i = 0; i < transform.parent.childCount; i++)
            {
                Destroy(transform.parent.GetChild(i).GetComponent<obs>());
            }
        }
    }
}
