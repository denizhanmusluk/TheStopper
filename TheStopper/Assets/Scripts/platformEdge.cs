using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platformEdge : MonoBehaviour
{
    float triggerTime = 0.5f;
    float counter = 0f;
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<playerBehaviour>() != null)
        {
            counter += Time.deltaTime;
            StartCoroutine(fallCheck(other.gameObject));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<playerBehaviour>() != null)
        {
            counter = 0;
        }
    }
    IEnumerator fallCheck(GameObject player)
    {
        yield return new WaitForSeconds(0.3f);
        if (!player.GetComponent<playerBehaviour>().playerFall && triggerTime< counter)
        {
            player.GetComponent<playerBehaviour>().playerFall = true;
            GameManager.Instance.Notify_LoseObservers();
            player.GetComponent<Ragdoll>().RagdollActivateWithForce(true, new Vector3(0, -1, 5));
        }
    }
}
