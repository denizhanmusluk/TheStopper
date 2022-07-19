using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jump : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<SwipeControl>() != null)
        {
            StartCoroutine(jumpPlayer(other.transform));
        }
    }
    IEnumerator jumpPlayer(Transform player)
    {
        player.GetComponent<Animator>().SetBool("jump", true);
        Globals.jumpActive = true;
        player.GetComponent<SwipeControl>().cam1.Follow = player;
        player.GetComponent<SwipeControl>().cam1.LookAt = player;
        if (!Input.GetMouseButton(0))
        {
            Globals.mousePress = true;
            yield return null;
            Globals.mousePress = false;
        }
        //yield return new WaitForSeconds(1f);

        float counter = 0f;
        float posY = 0f;
        while (counter < Mathf.PI)
        {
            player.GetComponent<Animator>().SetBool("jump", true);

            counter += 3 * Time.deltaTime;
            posY = Mathf.Sin(counter);
            player.position = new Vector3(player.position.x, posY * 20, player.position.z);
            yield return null;
        }
        player.GetComponent<Animator>().SetBool("jump", false);
        player.position = new Vector3(player.position.x,0, player.position.z);
        yield return new WaitForSeconds(1f);
        if (!Input.GetMouseButton(0))
        {
            Debug.Log("PRESS UP");
            Globals.mousePressUp = true;
            yield return null;
            Globals.mousePressUp = false;
        }
        Globals.jumpActive = false;
        player.GetComponent<SwipeControl>().cam1.Follow = player.parent;
        player.GetComponent<SwipeControl>().cam1.LookAt = player.parent;
    }
}
