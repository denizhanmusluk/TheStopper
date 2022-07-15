using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class playerBehaviour : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera cam;
    [SerializeField] GameObject particlePrefab;
    [SerializeField] GameObject leftFootPoint;
    [SerializeField] GameObject rightFootPoint;
    CinemachineBasicMultiChannelPerlin noise;
    public bool playerFall = false;
    void Start()
    {
        noise = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<item>() != null && other.GetComponent<item>().collectable)
        {
            other.GetComponent<item>().collectable = false;
            power.Instance.powerUpdate(other.GetComponent<item>().powerValue);
            other.GetComponent<item>().collect(this.transform);
        }
    }
    public void rightFoot()
    {
        StartCoroutine(shakeCam());
        GameObject smoke1 = Instantiate(particlePrefab, rightFootPoint.transform.position, rightFootPoint.transform.rotation);
        Destroy(smoke1, 1);
    }
    public void leftFoot()
    {
        StartCoroutine(shakeCam());
        GameObject smoke2 = Instantiate(particlePrefab, leftFootPoint.transform.position, leftFootPoint.transform.rotation);
        Destroy(smoke2, 1);
    }
    IEnumerator shakeCam()
    {
        float ampGain = 6;
        while (ampGain > 0)
        {
            ampGain -= 10 * Time.deltaTime;
            noise.m_AmplitudeGain = ampGain;

            yield return null;
        }
        noise.m_AmplitudeGain = 0;
    }
}
