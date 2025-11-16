using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System;

public class CardManager : MonoBehaviour
{
    [Header("Card Prefabs")]
    public GameObject cardPrefab;
    public Transform cardSpawnPoint;
    public Transform handTransform;

    public TextMeshProUGUI iron;
    public TextMeshProUGUI copper;
    public TextMeshProUGUI steel;
    public TextMeshProUGUI coal;
    public TextMeshProUGUI Adamantit;



    public int ironN;
    public int copperN;
    public int steelN;
    public int coalN;
    public int AdamantitN;

    public void SetIronN(int value)
    {
        ironN = value;
        data.iron = value;
    }

    public void SetCopperN(int value)
    {
        copperN = value;
        data.copper = value;
    }

    public void SetSteelN(int value)
    {
        steelN = value;
        data.steel = value;

    }

    public void SetCoalN(int value)
    {
        coalN = value;
        data.coal = value;

    }

    public void SetAdamantitN(int value)
    {
        AdamantitN = value;
        data.adamantium = value;

    }

    [Header("Available Cards")]
    public List<GameObject> availableCards = new List<GameObject>();


    public GameInfoDummy data;

    [Header("Info about Waves")]

    public WaveManager waveManager;


    

    public void Update()
    {
        ironN = data.iron;
        copperN = data.copper;
        steelN = data.steel;
        coalN = data.coal;
        AdamantitN = data.adamantium;

        iron.SetText(Convert.ToString(ironN));
        copper.SetText(Convert.ToString(copperN));
        steel.SetText(Convert.ToString(steelN));
        coal.SetText(Convert.ToString(coalN));
        Adamantit.SetText(Convert.ToString(AdamantitN));
    }

}