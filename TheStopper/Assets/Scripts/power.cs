using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class power : MonoBehaviour,ILoseObserver
{
    public static power Instance;

    [SerializeField] public GameObject sweatingParticle;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] SkinnedMeshRenderer playerMesh;
    [SerializeField] Material firstMaterial, redMaterial;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    void Start()
    {
        sweatingParticle.SetActive(false);
        Globals.playerColorActive = false;
        playerMesh.material = firstMaterial;
        redMaterial.color = firstMaterial.color;
        //Globals.power = PlayerPrefs.GetInt("power");
        levelText.text = ((int)Globals.power).ToString();
        GameManager.Instance.Add_LoseObserver(this);
    }

    public void LoseScenario()
    {
        GameManager.Instance.Remove_LoseObserver(this);
        transform.GetChild(0).gameObject.SetActive(false);
    }
    public void powerUpdate(int miktar)
    {
        StartCoroutine(Scaling(levelText.transform));
        int moneyOld = Globals.power;
        Globals.power = Globals.power + miktar;
        if(Globals.power < 0)
        {
            Globals.power = 0;
            Globals.isGameActive = false;
            GameManager.Instance.Notify_LoseObservers();
            transform.parent.GetComponent<Ragdoll>().RagdollActivateWithForce(true, new Vector3(0, -1, 5));
            transform.parent.GetComponent<SwipeControl>()._vehicle.currentState = vehicle.States.fail;
        }
        if (Globals.power < 20 && Globals.playerColorActive)
        {
            playerMesh.material = redMaterial;
            if (Globals.power > 10)
            {
                float green = (float)Globals.power / 20f;
                float blue = ((float)Globals.power - 10) / 10;
                redMaterial.color = new Color(1, green, blue);
            }
            else
            {
                ParticleSystem ps1 = sweatingParticle.transform.GetChild(0).GetComponent<ParticleSystem>();
                ParticleSystem ps2 = sweatingParticle.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>();
                var main1 = ps1.main;
                var main2 = ps2.main;
                main1.duration = 0.1f + (float)Globals.power / 10f;
                main2.duration = 0.1f + (float)Globals.power / 10f;
                float green = (float)Globals.power / 20f;
                float blue = 0;
                redMaterial.color = new Color(1, green, blue);
            }
        }
        else
        {
            playerMesh.material = firstMaterial;
        }
        //if (Globals.power <= 5)
        //{
        //    Time.timeScale = 0.2f;
        //}
        //else
        //{
        //    Time.timeScale = 1;
        //}
        LeanTween.value(moneyOld, Globals.power, 0.05f).setOnUpdate((float val) =>
        {
            levelText.text = ((int)val).ToString();
        });
        //PlayerPrefs.SetInt("power", Globals.power);

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
}
