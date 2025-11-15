using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Card", menuName = "Cards/Card Data")]
public class CardData : ScriptableObject
{
    [Header("Main Info")]
    public string cardName;
    public string description;
    public CardType cardType;

    [Header("Card UI References")]
    public Sprite cardBackground;     // фон карты
    public Sprite cardImage;          // арт карты
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI healthText;


    [Header("Stats")]
    public int manaCost;
    public int attack;
    public int health;

    [Header("Appearance")]
    public Sprite cardArt;          // главное изображение
    public Color tintColor = Color.white; // цветовая тонировка (если нужна)
}

public enum CardType
{
    Creature,
    Spell,
    Equipment,
    Resource
}
