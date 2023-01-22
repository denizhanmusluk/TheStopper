using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lights : MonoBehaviour
{
    [SerializeField] GameObject lights;
    void Start()
    {
        StartCoroutine(lightSet());
    }

   IEnumerator lightSet()
    {
        while (true)
        {
            lights.SetActive(true);
            yield return new WaitForSeconds(Random.Range(0.2f, 1f));
            lights.SetActive(false);
            yield return new WaitForSeconds(Random.Range(0.2f, 0.4f));
        }
    }
}
