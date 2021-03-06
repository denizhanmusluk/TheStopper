using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;

public class vehicle : MonoBehaviour,IStartGameObserver,ILoseObserver
{
    [SerializeField] SkinnedMeshRenderer _skinnedMeshRenderer;
    public enum States { follow, push ,win,fail}
    public States currentState;
    [SerializeField] Transform followingPoint, targetPlayer;
    public bool hitting = false;
    public bool push = false;
    [SerializeField] float hitPeriod;
    float hitPeriodFirst;

    [SerializeField] TextMeshProUGUI vehiclelevelText;
    [SerializeField] public int vehiclePower;
    [SerializeField] float maxPower;
    int vehicleFirstHitPower;
    Animator anim;
    [SerializeField] float pushSpeed;
    [SerializeField] float currentPushSpeed;
    [SerializeField] GameObject crashParticle;

    [SerializeField] public Transform firstCamPosition, lastCamPosition;
    Vector3 camMoveDirection;
    float camFactorDistance;
    [SerializeField] public
    CinemachineVirtualCamera cam;
    [SerializeField] Material groundMat;
    Vector3 vehicleFirstPos;
   public float followSpeed = 90;
    [SerializeField] GameObject windParticle;
    [SerializeField] GameObject tireSmokes;
    [SerializeField] ParticleSystem[] windParticles;
    void Start()
    {
        Globals.finish = false;
        tireSmokes.SetActive(true);
        vehicleFirstPos = transform.position;
        StartCoroutine(firstAnim());
        StartCoroutine(tiling());
        GameManager.Instance.Add_StartObserver(this);
        GameManager.Instance.Add_LoseObserver(this);
        maxPower = vehiclePower;
        power.Instance.MaxPowerSize = vehiclePower * 1.3f;
        hitPeriodFirst = hitPeriod;
        _skinnedMeshRenderer.SetBlendShapeWeight(0, 0);
        if (GetComponent<Animator>() != null)
        {
            anim = GetComponent<Animator>();
        }
        StartCoroutine(animatorSet());

        vehiclelevelText.text = vehiclePower.ToString();


        cam.transform.position = lastCamPosition.position;
        camMoveDirection = (lastCamPosition.position - firstCamPosition.position).normalized;
        camFactorDistance = Vector3.Distance(lastCamPosition.position, firstCamPosition.position);
    }
    public void LoseScenario()
    {
        currentState = States.fail;
    }
    IEnumerator animatorSet()
    {
        yield return new WaitForSeconds(1.33f);
        if (GetComponent<Animator>() != null)
        {
            anim.enabled = false;
        }
    }
    IEnumerator tiling()
    {
        float counter = 0f;
        while (!Globals.isGameActive)
        {
            counter += 0.5f * Time.deltaTime;
            groundMat.mainTextureOffset = new Vector2(0, -counter);
            yield return null;
        }
        groundMat.mainTextureOffset = new Vector2(0, 0);
    }
    IEnumerator firstAnim()
    {
        float counter = 0f;
        while (!Globals.isGameActive)
        {
            float distance = Random.Range(10f, 15f);
            float speed = Random.Range(0.5f, 1.5f);
            while (counter < Mathf.PI / 2)
            {
                counter += speed* Time.deltaTime;
                float val = Mathf.Abs(Mathf.Sin(counter));
                transform.position = new Vector3(transform.position.x, transform.position.y, vehicleFirstPos.z + val * distance);
                if (Globals.isGameActive)
                {
                    break;
                }
                yield return null;
            }

            if (GetComponent<Animator>() != null)
            {
                anim.enabled = true;
                anim.SetBool("push", true);
            }
            if (Globals.isGameActive)
            {
                break;
            }
            yield return new WaitForSeconds(Random.Range(1f, 3f));
            if (Globals.isGameActive)
            {
                break;
            }
            if (GetComponent<Animator>() != null)
            {
                anim.enabled = false;
                anim.SetBool("push", false);
            }
            yield return new WaitForSeconds(Random.Range(1f, 3f));

            while (counter < Mathf.PI)
            {
                if (Globals.isGameActive)
                {
                    break;
                }

                counter += 0.3f* Time.deltaTime;
                float val = Mathf.Abs(Mathf.Sin(counter));
                transform.position = new Vector3(transform.position.x, transform.position.y, vehicleFirstPos.z + val * distance);
                yield return null;
            }
            counter = 0f;
            if (GetComponent<Animator>() != null)
            {
                anim.enabled = true;
                anim.SetBool("push", true);
            }
            if (Globals.isGameActive)
            {
                break;
            }
            yield return new WaitForSeconds(Random.Range(0, 4f));
            if (Globals.isGameActive)
            {
                break;
            }
            if (GetComponent<Animator>() != null)
            {
                anim.SetBool("push", false);
                anim.enabled = false;
            }
        }
    }
    public void StartGame()
    {
        //anim.enabled = true;
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
        transform.position = Vector3.MoveTowards(transform.position, followingPoint.position, followSpeed * Time.deltaTime);
    }
    void moveToTarget()
    {
        if (Vector3.Distance(transform.position,targetPlayer.GetChild(1).position) < 0.1f && !hitting)
        {
            Debug.Log("hit");
            hitting = true;
            push = true;
            StartCoroutine(hitPlayer());
            StartCoroutine(pushPlayer());
            StartCoroutine(pushMoveSpeed());
        }
        transform.position = Vector3.MoveTowards(transform.position, targetPlayer.GetChild(1).position, 120 * Time.deltaTime);

    }
    void runOver()
    {
        hitting = false;
        transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(0, 0, 10), 75 * Time.deltaTime);
    }
    IEnumerator hitPlayer()
    {
        hitPeriod = 1f;

        vehicleFirstHitPower = vehiclePower;
        float counter = 0f;
        float angle = 0f;

        targetPlayer.transform.GetChild(1).localPosition = new Vector3(targetPlayer.transform.GetChild(1).localPosition.x, targetPlayer.transform.GetChild(1).localPosition.y, -0.5f);

        //transform.position += new Vector3(0, -5, 0);
        while (counter < Mathf.PI)
        {
            targetPlayer.transform.GetChild(1).localPosition = Vector3.MoveTowards(targetPlayer.transform.GetChild(1).localPosition, new Vector3(targetPlayer.transform.GetChild(1).localPosition.x, targetPlayer.transform.GetChild(1).localPosition.y, 0), Time.deltaTime);

            //transform.position = new Vector3(transform.position.x, -4, targetPlayer.GetChild(1).position.z - 2);
            counter += 5 * Time.deltaTime;
            angle = 15 * Mathf.Sin(counter);
            transform.rotation = Quaternion.Euler(angle, 0, 0);
            yield return null;
        }
        transform.rotation = Quaternion.Euler(0, 0, 0);

        targetPlayer.transform.GetChild(1).localPosition = new Vector3(targetPlayer.transform.GetChild(1).localPosition.x, targetPlayer.transform.GetChild(1).localPosition.y, 0);
        //yield return new WaitForSeconds(1f);
        LeanTween.value(1, hitPeriodFirst, 1).setOnUpdate((float val) =>
        {
            hitPeriod = val;
        });
        hitPeriod = hitPeriodFirst;

        //transform.position = new Vector3(transform.position.x, 0, transform.position.z);
    }
    IEnumerator pushPlayer()
    {
        windParticle.SetActive(true);
        if (GetComponent<Animator>() != null)
        {
            anim.enabled = true;
            anim.SetBool("push", true);
        }

        //yield return new WaitForSeconds(0.5f);
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
                //hitPeriod = hitPeriodFirst;
            }
            else
            {
                //hitPeriod -= 0.001f;
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
                tireSmokes.SetActive(false);
                crashParticle.SetActive(true);
                Destroy(crashParticle, 4);
                GetComponent<Ragdoll>().RagdollActivateWithForce(true, new Vector3(0,2,2));
                currentState = States.win;
                hitting = false;
                Globals.finish = true;
                GameManager.Instance.Notify_WinObservers();
                break;

            }
            if (vehiclePower / maxPower < 1f / 4f)
            {
                _skinnedMeshRenderer.SetBlendShapeWeight(0, 200f * ((2f / 4f) - (vehiclePower / maxPower)));
            }
            else if (vehiclePower / maxPower < 3f / 4f)
            {
                _skinnedMeshRenderer.SetBlendShapeWeight(0, 100f * ((3f / 4f) - (vehiclePower / maxPower)));
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
            // cam.transform.position = new Vector3((0.5f * camFactorDistance * camMoveDirection.x) + firstCamPosition.position.x, ( 0.5f * camFactorDistance * camMoveDirection.y) + firstCamPosition.position.y, (0.5f * camFactorDistance * camMoveDirection.z) + firstCamPosition.position.z);
            cam.transform.position = Vector3.MoveTowards(cam.transform.position, new Vector3(((1 - (float)vehiclePower / (float)vehicleFirstHitPower) * camFactorDistance * camMoveDirection.x) + firstCamPosition.position.x, ((1 - (float)vehiclePower / (float)vehicleFirstHitPower) * camFactorDistance * camMoveDirection.y) + firstCamPosition.position.y, ((1 - (float)vehiclePower / (float)vehicleFirstHitPower) * camFactorDistance * camMoveDirection.z) + firstCamPosition.position.z),50* Time.deltaTime);
            cam.m_Lens.FieldOfView = (130- (30 - 30 * (float)vehiclePower / (float)vehicleFirstHitPower));

            yield return new WaitForSeconds(hitPeriod);
        }
        windParticle.SetActive(false);

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

        //yield return new WaitForSeconds(0.5f);
        while (hitting && push)
        {
            for(int i = 0;i< windParticles.Length; i++)
            {
                var _main = windParticles[i].main;
                _main.simulationSpeed = 2 * (float)vehiclePower / (float)vehicleFirstHitPower;
            }
            targetPlayer.parent.Translate(Vector3.forward * Time.deltaTime * (float)vehiclePower / (float)vehicleFirstHitPower * pushSpeed);
            yield return null;
        }
    }
}
