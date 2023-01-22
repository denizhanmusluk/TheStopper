using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jumpArea : MonoBehaviour
{
    Material groundMat;
    void Start()
    {
        groundMat = GetComponent<MeshRenderer>().material;
        StartCoroutine(tiling());
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
}
