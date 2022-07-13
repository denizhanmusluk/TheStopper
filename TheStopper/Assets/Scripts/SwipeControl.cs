using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class SwipeControl : MonoBehaviour,ILoseObserver,IWinObserver
{
    [SerializeField] public float bounding;
    [SerializeField] public float steeringSpeed = 180;
    [SerializeField] public float maxAngle;
    [Range(0.0f, 10.0f)]  [SerializeField] float Controlsensivity;
    public float moveSpeed = 15;

    private float m_previousX;
    private float dX;
    public enum States {idle, forward, backStop }
    public States currentBehaviour;
    Vector3 playerFirstPos;


    public float Xmove;

    bool runActive = true;
    bool pressActive = false;
    Animator anim;
    float firstSpeed;
    [SerializeField] CinemachineVirtualCamera cam1, cam2, cam3;
    [SerializeField] CinemachineVirtualCamera failCam;
    public vehicle _vehicle;
    bool swipeActive = true;
    int pushAnimSelect = 0;
    private void Start()
    {
        GameManager.Instance.Add_LoseObserver(this);
        GameManager.Instance.Add_WinObserver(this);

        cam1.Priority = 0;
        cam2.Priority = 1;
        cam3.Priority = 0;
        firstSpeed = moveSpeed;
        moveSpeed = 0f;
        anim = GetComponent<Animator>();
        playerFirstPos = transform.position;

    }
    public void LoseScenario()
    {
        GameManager.Instance.Remove_LoseObserver(this);
        failCam.Priority = 10;
        failCam.LookAt = transform.GetChild(2).GetChild(0).transform;

    }
    public void WinScenario()
    {
        anim.SetTrigger("finish");
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && runActive && Globals.isGameActive)
        {
            transform.GetChild(1).localPosition = new Vector3(transform.GetChild(1).localPosition.x, transform.GetChild(1).localPosition.y, 0);
            pressActive = true;
            m_previousX = Input.mousePosition.x;
            dX = 0f;
            anim.SetBool("run", true);
            pushAnimSelect = Random.Range(0, 2);
            string[] pushAn = { "push1", "push2" };
            anim.SetTrigger(pushAn[pushAnimSelect]);
            StartCoroutine(run());
            StartCoroutine(timeSet());
            _vehicle.push = false;
            power.Instance.sweatingParticle.SetActive(false);
        }
        if (Input.GetMouseButtonUp(0) && runActive && pressActive && Globals.isGameActive)
        {
            dX = 0f;
            runActive = false;
            pressActive = false;

            anim.SetBool("run", false);
            StartCoroutine(Stop());
            StartCoroutine(runActivation());

        }


        switch (currentBehaviour)
        {

            case States.idle:
                {

                }
                break;
            case States.forward:
                {
                    if (Globals.isGameActive)
                    {
                        if (swipeActive)
                        {
                            controlUpdate();
                        }
                        transform.parent.transform.Translate(transform.parent.transform.forward * Time.deltaTime * moveSpeed);
                    }
                }
                break;
            case States.backStop:
                {

                }
                break;


        }
    }
    IEnumerator timeSet()
    {
        yield return new WaitForSeconds(0.5f);
   
            Time.timeScale = 1;
        
    }
    IEnumerator runActivation()
    {
        yield return new WaitForSeconds(1f);
        runActive = true;
    }
    IEnumerator Stop()
    {
        cam1.Priority = 0;
        cam2.Priority = 1;
        cam3.Priority = 0;
        if (pushAnimSelect == 1)
        {
            transform.GetChild(1).localPosition = new Vector3(transform.GetChild(1).localPosition.x, transform.GetChild(1).localPosition.y, 1f);
        }
            float counter = 0f;
        float waitingTime = 0.5f;
        while (counter < waitingTime)
        {
            counter += Time.deltaTime;
            if (moveSpeed > 0)
            {
                moveSpeed -= (60 / waitingTime) * Time.deltaTime;
            }
            else
            {
                moveSpeed = 0;
            }
            transform.parent.transform.Translate(transform.parent.transform.forward * Time.deltaTime * moveSpeed);


            //transform.parent.position = Vector3.MoveTowards(transform.parent.position, new Vector3(0, transform.parent.position.y, transform.parent.position.z), 15 * Time.deltaTime);
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, new Vector3(0, transform.localPosition.y, transform.localPosition.z), Time.deltaTime);

            yield return null;
        }
        _vehicle.currentState = vehicle.States.push;

        currentBehaviour = States.backStop;
        StartCoroutine(turnBack());
        moveSpeed = 0;
    }
    IEnumerator turnBack()
    {
        float counter = 0f;
        float angle = 180;
        yield return new WaitForSeconds(0.1f);
        while (counter < angle)
        {
            counter +=540 * Time.deltaTime;

            transform.rotation = Quaternion.Euler(0, 0, 0);
            if (pushAnimSelect == 1)
            {
                transform.rotation = Quaternion.Euler(0, counter, 0);
            }


            transform.parent.position = Vector3.MoveTowards(transform.parent.position, new Vector3(0, transform.parent.position.y, transform.parent.position.z), 8 * Time.deltaTime);
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, new Vector3(0, transform.localPosition.y, transform.localPosition.z), Time.deltaTime);
            yield return null;
        }
        if (pushAnimSelect == 1)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        yield return new WaitForSeconds(1f);
        if (!pressActive)
        {
            cam1.Priority = 0;
            cam2.Priority = 0;
            cam3.Priority = 1;
        }
    }
    IEnumerator run()
    {
        swipeActive = false;
        currentBehaviour = States.forward;

        cam1.Priority = 1;
        cam2.Priority = 0;
        cam3.Priority = 0;
        float counter = 0f;
        float waitingTime = 0.5f;
        while (counter < waitingTime)
        {
            counter += Time.deltaTime;
            moveSpeed += (firstSpeed/ waitingTime) * Time.deltaTime;
            //transform.parent.transform.Translate(transform.parent.transform.forward * Time.deltaTime * moveSpeed);
            if (!runActive)
            {
                break;
            }
            dX = 0f;
            yield return null;
        }
        if (runActive)
        {
            moveSpeed = firstSpeed;
        }

        _vehicle.currentState = vehicle.States.follow;
        swipeActive = true;
        m_previousX = Input.mousePosition.x;
        dX = 0f;

    }
    private void controlUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //m_previousX = Input.mousePosition.x;
            //dX = 0f;
        }
        if (Input.GetMouseButton(0))
        {
            dX = (Input.mousePosition.x - m_previousX) / 10f;
            m_previousX = Input.mousePosition.x;
        }
        if (Input.GetMouseButtonUp(0))
        {
            //dX = 0f;
        }
        Xmove = Controlsensivity * dX / (Time.deltaTime * 25);
        Move(Xmove);
    }
    public void Move(float _swipe)
    {
        if (_swipe > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(playerFirstPos.x + bounding, transform.position.y, transform.position.z), Time.deltaTime * Mathf.Abs(_swipe));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, maxAngle, transform.eulerAngles.z), steeringSpeed * Time.deltaTime);


            //transform.parent.position = Vector3.MoveTowards(transform.parent.position, new Vector3(playerFirstPos.x + bounding * 0.4f, transform.position.y, transform.position.z),0.4f* Time.deltaTime * Mathf.Abs(_swipe));

        }
        if (_swipe < 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(playerFirstPos.x - bounding, transform.position.y, transform.position.z), Time.deltaTime * Mathf.Abs(_swipe));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, -maxAngle, transform.eulerAngles.z), steeringSpeed * Time.deltaTime);

            //transform.parent.position = Vector3.MoveTowards(transform.parent.position, new Vector3(playerFirstPos.x - bounding * 0.4f, transform.position.y, transform.position.z), 0.4f * Time.deltaTime * Mathf.Abs(_swipe));

        }
        if (_swipe == 0)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, 0, transform.eulerAngles.z), 2 * steeringSpeed * Time.deltaTime);
        }

        transform.parent.position = new Vector3(transform.localPosition.x * 8f, transform.parent.position.y, transform.parent.position.z);

    }

}
