using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platformCreator : MonoBehaviour
{
    [SerializeField] GameObject[] platforms;
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<playerBehaviour>() != null)
        {
            //GetComponent<Collider>().enabled = false;
            int platformSelect = Random.Range(0, platforms.Length);
            Instantiate(platforms[platformSelect], transform.position + new Vector3(0, 0, 2000), Quaternion.identity);
            transform.position += new Vector3(0, 0, 2000);
        }
    }
}
