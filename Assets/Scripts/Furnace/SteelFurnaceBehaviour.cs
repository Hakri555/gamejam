using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteelFurnaceBehaviour : MonoBehaviour
{
    [SerializeField] ProgressBarLogic progressBarLogic;
    [SerializeField] GameInfoDummy gameInfoDummy;
    [SerializeField] float smeltingTime;
    [SerializeField] int ironInput;
    [SerializeField] int coalInput;
    [SerializeField] int output;
    private bool isSmelting;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    public void StartSmelting()
    {
        if (!isSmelting && gameInfoDummy.coal >= coalInput && gameInfoDummy.iron >= ironInput)
        {
            StartCoroutine(WaitWhileSmelting(smeltingTime));
        }
    }

    IEnumerator WaitWhileSmelting(float time)
    {
        gameInfoDummy.coal -= coalInput;
        gameInfoDummy.iron -= ironInput;
        isSmelting = true;
        progressBarLogic.ShowProgress(smeltingTime);
        yield return new WaitForSeconds(time);
        isSmelting = false;
        gameInfoDummy.steel += output;
    }
}
