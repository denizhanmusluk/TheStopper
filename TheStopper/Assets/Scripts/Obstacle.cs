using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] GameObject demolishParticle;
    public bool particleActice = false;
    [SerializeField] Vector3 rotAx;
    [SerializeField] Transform rotPart;
    [SerializeField] float rotSpeed;
    [SerializeField] bool rotationActive;
    private void Start()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<vehicle>() != null || other.GetComponent<SwipeControl>() != null)
        {
            triggerFunction(other);
        }
    }
    public void triggerFunction(Collider other)
    {
        rotationActive = false;
        obstacleHit(other.gameObject);
        GetComponent<Rigidbody>().detectCollisions = false;
        //GetComponent<Collider>().enabled = false;
        if (other.GetComponent<playerBehaviour>() != null && !Globals.pushActive)
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
        foreach (var rigidb in GetComponentsInChildren<Rigidbody>())
        {
            //Vector3 forceDirection = (rigidb.transform.position - man.transform.position).normalized + Vector3.forward;
            rigidb.transform.GetComponent<Rigidbody>().isKinematic = false;
            rigidb.transform.GetComponent<Rigidbody>().useGravity = true;
            rigidb.transform.GetComponent<Rigidbody>().mass = 2;
            rigidb.transform.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-3f,3f) , Random.Range(-3f, 3f), Random.Range(-3f, 3f)) * 1000 );
            rigidb.transform.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), Random.Range(-3f, 3f)) * 500);
            rigidb.GetComponent<Collider>().isTrigger = false;
        }
        if (GetComponent<Rigidbody>() != null)
        {
            GetComponent<Rigidbody>().detectCollisions = false;
        }
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).transform.GetComponent<Rigidbody>() == null)
            {
                //Vector3 obsPos = new Vector3(-1 / glassBroken.transform.GetChild(i).transform.localPosition.y, 1, -1 / (glassBroken.transform.GetChild(i).transform.localPosition.y));
                Vector3 forceDirection = (transform.GetChild(i).transform.position - man.transform.position).normalized + Vector3.forward;
                transform.GetChild(i).gameObject.AddComponent<Rigidbody>();
                transform.GetChild(i).transform.GetComponent<Rigidbody>().useGravity = true;
                transform.GetChild(i).transform.GetComponent<Rigidbody>().mass = 2;
                transform.GetChild(i).transform.GetComponent<Rigidbody>().AddForce(forceDirection * 1000 * (10 / Vector3.Distance(transform.GetChild(i).transform.position, man.transform.position)));
                transform.GetChild(i).transform.GetComponent<Rigidbody>().AddTorque(forceDirection * 500);
            }
        }
        Destroy(gameObject, 4);
    }
    private void Update()
    {
        if (rotationActive)
        {
            rotPart.Rotate(rotAx * rotSpeed * Time.deltaTime);
        }
    }
}
