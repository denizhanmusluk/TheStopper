using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemSpawner : MonoBehaviour, IStartGameObserver
{
    public GameObject[] coin;
    public int coinGroupCount = 10;
    public int coinGroupDistance = 10;

    private float[] xPosition = new float[2];
    private float[] yPosition = new float[2];
    void Start()
    {
        GameManager.Instance.Add_StartObserver(this);
        if (Globals.isGameActive)
        {
            StartCoroutine(_coinSpawn());
        }
    }
    public void StartGame()
    {
        GameManager.Instance.Remove_StartObserver(this);
        StartCoroutine(_coinSpawn());
    }

    IEnumerator _coinSpawn()
    {
        xPosition[0] = 0f;
        xPosition[1] = 0f;

        yPosition[0] = -0.7f;
        yPosition[1] = 0.7f;

        for (int x = 1; x <= coinGroupCount; x++)
        {
            int xIndex = Random.Range(0, 2);
            int selectionCoin = Random.Range(0, coin.Length);
     
            float zPos = x * coinGroupDistance;
            var diamond = Instantiate(coin[selectionCoin], transform.position + new Vector3(0, 0, zPos), Quaternion.identity);
            //diamond.transform.parent = gameObject.transform;
            if (x > 4)
            {
                yield return new WaitForSeconds(1.5f);
            }
        }
    }
}
