using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellishSteelFurnaceBehaviour : MonoBehaviour
{
    [SerializeField] ProgressBarLogic progressBarLogic;
    [SerializeField] GameInfoDummy gameInfoDummy;
    [SerializeField] float smeltingTime;
    [SerializeField] int steelInput;
    [SerializeField] int copperInput;
    [SerializeField] int adamantiumInput;
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
        if (!isSmelting && gameInfoDummy.coal >= coalInput && gameInfoDummy.steel >= steelInput && gameInfoDummy.copper >= copperInput && gameInfoDummy.adamantium >= adamantiumInput)
        {
            StartCoroutine(WaitWhileSmelting(smeltingTime));
        }
    }

    IEnumerator WaitWhileSmelting(float time)
    {
        gameInfoDummy.coal -= coalInput;
        gameInfoDummy.copper -= copperInput;
        gameInfoDummy.adamantium -= adamantiumInput;
        gameInfoDummy.steel -= steelInput;
        isSmelting = true;
        progressBarLogic.ShowProgress(smeltingTime);
        yield return new WaitForSeconds(time);
        isSmelting = false;
        gameInfoDummy.hellishSteel += output;
    }
}
