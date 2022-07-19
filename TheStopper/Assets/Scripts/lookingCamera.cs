using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lookingCamera : MonoBehaviour,IWinObserver
{
    [SerializeField] GameObject cameraObject;
    private void Start()
    {
        GameManager.Instance.Add_WinObserver(this);
        cameraObject.transform.parent.GetComponent<Collider>().enabled = false;
    }
    void LateUpdate()
    {
        transform.LookAt(cameraObject.transform);
    }
    public void WinScenario()
    {
        cameraObject.transform.parent.GetComponent<Collider>().enabled = true;
    }
}
