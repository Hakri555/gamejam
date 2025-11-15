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


    public int ironN;
    public int copperN;
    public int steelN;



    [Header("Available Cards")]
    public List<GameObject> availableCards = new List<GameObject>();


    [Header("Deck Settings")]
    public int initialHandSize = 5;

    private List<GameObject> currentHand = new List<GameObject>();



    public void Awake()
    {



        
    }


    public void Update()
    {
        iron.SetText(Convert.ToString(ironN));
        copper.SetText(Convert.ToString(copperN));
        steel.SetText(Convert.ToString(steelN));
    }



}