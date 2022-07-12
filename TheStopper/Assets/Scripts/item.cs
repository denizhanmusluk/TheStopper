using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item : MonoBehaviour
{
    public int powerValue;
    public bool collectable = true;
    int[] rotateDirection = { -1, 1 };
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
}
