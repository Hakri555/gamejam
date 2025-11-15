using UnityEngine;
using System.Collections.Generic;

public class CardManager : MonoBehaviour
{
    [Header("Card Prefabs")]
    public GameObject cardPrefab;
    public Transform cardSpawnPoint;
    public Transform handTransform;

    [Header("Available Cards")]
    public List<CardData> availableCards = new List<CardData>();

    [Header("Deck Settings")]
    public int initialHandSize = 5;

    private List<GameObject> currentHand = new List<GameObject>();


    
}