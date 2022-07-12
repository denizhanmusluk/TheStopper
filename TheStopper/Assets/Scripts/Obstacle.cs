using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] GameObject demolishParticle;
    public bool particleActice = false;
    private void Start()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        obstacleHit(other.gameObject);
        GetComponent<Collider>().enabled = false;
        if (other.GetComponent<playerBehaviour>() != null)
        {
            power.Instance.powerUpdate(-damage);
        }
    }
    public void obstacleHit(GameObject man)
    {
        //if (particleActice)
        //{
        //    GameObject yearUp = Instantiate(yearUpParticle, transform.position + new Vector3(0, 1, 0), Quaternion.Euler(-90, 0, 0));
        //    yearUp.transform.localScale = new Vector3(7, 7, 7);
        //}
  
        //transform.GetChild(2).gameObject.SetActive(false);
        //transform.GetComponent<BoxCollider>().enabled = false;

        for (int i = 0; i < transform.childCount; i++)
        {
            //Vector3 obsPos = new Vector3(-1 / glassBroken.transform.GetChild(i).transform.localPosition.y, 1, -1 / (glassBroken.transform.GetChild(i).transform.localPosition.y));
            Vector3 forceDirection = (transform.GetChild(i).transform.position - man.transform.position).normalized + Vector3.forward;
            transform.GetChild(i).gameObject.AddComponent<Rigidbody>();
            transform.GetChild(i).transform.GetComponent<Rigidbody>().useGravity = true;
            transform.GetChild(i).transform.GetComponent<Rigidbody>().mass = 2;
            transform.GetChild(i).transform.GetComponent<Rigidbody>().AddForce(forceDirection * 1000 * (10 / Vector3.Distance(transform.GetChild(i).transform.position, man.transform.position)));
            transform.GetChild(i).transform.GetComponent<Rigidbody>().AddTorque(forceDirection * 500);
        }
        Destroy(gameObject, 4);
    }
}
