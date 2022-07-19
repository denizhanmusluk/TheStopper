using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item : MonoBehaviour
{
    public int powerValue;
    public bool collectable = true;
    int[] rotateDirection = { -1, 1 };
    Transform target;
    float motionSpeed = 50;
    private void Start()
    {
        StartCoroutine(rotation());
    }
    public void collectItem()
    {
        StartCoroutine(collect());
    }
    IEnumerator collect()
    {

        float counter = 0f;
        float val = 0f;
        float firstPosY = transform.position.y;
        while (counter < 3 * Mathf.PI / 2)
        {
            counter += 10 * Time.deltaTime;
            val = Mathf.Sin(counter);
            transform.position = new Vector3(transform.position.x, firstPosY + val * 3 * counter, transform.position.z);

            yield return null;
        }
        Destroy(gameObject);
    }
    IEnumerator rotation()
    {
        int rotDir = rotateDirection[Random.Range(0, 2)];
        float counter = 0;
        float startDelay = Random.Range(0f, 1f);
        yield return new WaitForSeconds(startDelay);
        while (true)
        {
            counter += 200 * Time.deltaTime;
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, rotDir * counter, transform.eulerAngles.z);
            yield return null;
        }
    }
    IEnumerator targetMotion()
    {
        //particle.SetActive(false);

        GetComponent<SphereCollider>().enabled = false;
        float counter = 0f;
        float angle = 0f;
        Vector3 dirVect = (transform.position - target.position).normalized;



        while (counter < Mathf.PI / 2)
        {
            counter += 6 * Time.deltaTime;
            angle = 100 * Mathf.Cos(counter);
            dirVect = new Vector3(dirVect.x, 0.3f, dirVect.z);
            transform.position = Vector3.MoveTowards(transform.position, transform.position + dirVect * angle, counter * motionSpeed * Time.deltaTime);
            transform.localScale = new Vector3(transform.localScale.x - 0.5f * Time.deltaTime, transform.localScale.y - 0.5f * Time.deltaTime, transform.localScale.z - 0.5f * Time.deltaTime);

            //transform.localScale -= new Vector3(0.005f, 0.005f, 0.005f);

            //transform.position = Vector3.MoveTowards(transform.position, transform.position + dirVect * angle + new Vector3(0, 0.5f, 0), counter * motionSpeed * Time.deltaTime);
            yield return null;
        }

        while (Vector3.Distance(transform.position, target.position + new Vector3(0,15,0)) > 1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position + new Vector3(0, 15, 0), (1 + Mathf.Abs(3 - 0.1f * Vector3.Distance(transform.position, target.position + new Vector3(0, 15, 0)))) * motionSpeed * Time.deltaTime);
            transform.localScale = new Vector3(transform.localScale.x - 0.5f * Time.deltaTime, transform.localScale.y - 0.5f * Time.deltaTime, transform.localScale.z - 0.5f * Time.deltaTime);
            //transform.position = Vector3.MoveTowards(transform.position, target.position, (40 / Vector3.Distance(transform.position, target.position)) * motionSpeed * Time.deltaTime);
            yield return null;
        }
        //TapticManager.Impact(ImpactFeedback.Light);



        //Destroy(particle);

        //target.GetComponent<PlayerParent>().playerYearSet(1);
        GameObject money = gameObject;
        money.transform.parent = null;
        Destroy(money);
    }
    public void collect(Transform moneyTarget)
    {
        transform.parent = moneyTarget;
        target = moneyTarget;
        StartCoroutine(targetMotion());
    }
}
