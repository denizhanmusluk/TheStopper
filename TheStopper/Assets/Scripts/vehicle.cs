using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class vehicle : MonoBehaviour
{
    public enum States { follow, push ,win,fail}
    public States currentState;
    [SerializeField] Transform followingPoint, targetPlayer;
  public  bool hitting = false;
  public  bool push = false;
    [SerializeField] float hitPeriod;

    [SerializeField] TextMeshProUGUI vehiclelevelText;
    [SerializeField] int vehiclePower;
     int vehicleFirstHitPower;
    Animator anim;
    [SerializeField] float pushSpeed;
    [SerializeField] float currentPushSpeed;
    [SerializeField] GameObject crashParticle;
    void Start()
    {
        if (GetComponent<Animator>() != null)
        {
            anim = GetComponent<Animator>();
        }
        vehiclelevelText.text = vehiclePower.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {

            case States.follow:
                {
                    if (Globals.isGameActive)
                    {
                        following();
                    }
                }
                break;
            case States.push:
                {
                    moveToTarget();
                }
                break;
            case States.win:
                {
                    

                }
                break;
            case States.fail:
                {
                    runOver();
                }
                break;
        }
    }
    void following()
    {
        if (GetComponent<Animator>() != null)
        {
            anim.SetBool("push", false);
            anim.enabled = false;
        }
        hitting = false;
        transform.position = Vector3.MoveTowards(transform.position, followingPoint.position, 90 * Time.deltaTime);
    }
    void moveToTarget()
    {
        if (Vector3.Distance(transform.position,targetPlayer.position) < 0.1f && !hitting)
        {
            Debug.Log("hit");
            hitting = true;
            push = true;
            StartCoroutine(hitPlayer());
            StartCoroutine(pushPlayer());
            StartCoroutine(pushMoveSpeed());
        }
        transform.position = Vector3.MoveTowards(transform.position, targetPlayer.position, 75 * Time.deltaTime);

    }
    void runOver()
    {
        hitting = false;
        transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(0, 0, 10), 75 * Time.deltaTime);
    }
    IEnumerator hitPlayer()
    {
        vehicleFirstHitPower = vehiclePower;
        float counter = 0f;
        float angle = 0f;
        transform.position += new Vector3(0, -5, 0);
        while(counter < Mathf.PI)
        {
            transform.position = new Vector3(transform.position.x, -4, targetPlayer.position.z - 2);
            counter += 5 * Time.deltaTime;
            angle = 20 * Mathf.Sin(counter);
            transform.rotation = Quaternion.Euler(angle, 0, 0);
            yield return null;
        }
        transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
    }
    IEnumerator pushPlayer()
    {
        if (GetComponent<Animator>() != null)
        {
            anim.enabled = true;
            anim.SetBool("push", true);
        }

        yield return new WaitForSeconds(0.5f);
        while (hitting && push)
        {
            if (Globals.power <= 5)
            {
                Time.timeScale = 0.2f;
            }
            if (Globals.power < 20)
            {
                power.Instance.sweatingParticle.SetActive(true);
                Globals.playerColorActive = true;
            }       
            if (Globals.power == 0)
            {
                power.Instance.sweatingParticle.SetActive(false);

                if (GetComponent<Animator>() != null)
                    anim.SetBool("push", false);
                yield return null;
                if (GetComponent<Animator>() != null)
                    anim.enabled = false;
                currentState = States.fail;
                hitting = false;
                GameManager.Instance.Notify_LoseObservers();
                targetPlayer.GetComponent<Ragdoll>().RagdollActivateWithForce(true, new Vector3(0,-1,5));
                break;
            }
            if (vehiclePower == 0)
            {
                crashParticle.SetActive(true);
                Destroy(crashParticle, 4);
                GetComponent<Ragdoll>().RagdollActivateWithForce(true, new Vector3(0,2,2));
                currentState = States.win;
                hitting = false;
                GameManager.Instance.Notify_WinObservers();
                break;

            }
            power.Instance.powerUpdate(-1);

            StartCoroutine(Scaling(vehiclelevelText.transform));
            int moneyOld = vehiclePower;
            vehiclePower = vehiclePower - 1;
            LeanTween.value(moneyOld, vehiclePower, 0.05f).setOnUpdate((float val) =>
            {
                vehiclelevelText.text = ((int)val).ToString();
            });
            //pushMoveSpeed((float)vehiclePower/(float)vehicleFirstHitPower);
            yield return new WaitForSeconds(hitPeriod);
        }
    }



    IEnumerator Scaling(Transform bagel)
    {
        yield return new WaitForSeconds(0.1f);
        float counter = 0f;
        float firstSize = 1f;
        float sizeDelta;
        while (counter < Mathf.PI)
        {
            counter += 20 * Time.deltaTime;
            sizeDelta = 1f - Mathf.Abs(Mathf.Cos(counter));
            sizeDelta /= 2f;
            bagel.GetComponent<RectTransform>().localScale = new Vector3(firstSize + sizeDelta, firstSize + sizeDelta, firstSize + sizeDelta);

            yield return null;
        }
        bagel.localScale = new Vector3(firstSize, firstSize, firstSize);
    }
    IEnumerator pushMoveSpeed()
    {

        yield return new WaitForSeconds(0.5f);
        while (hitting && push)
        {
            targetPlayer.parent.Translate(Vector3.forward * Time.deltaTime * (float)vehiclePower / (float)vehicleFirstHitPower * pushSpeed);
            yield return null;
        }
    }
}
