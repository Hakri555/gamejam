using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;

public class DraggableCard : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler, IPointerEnterHandler
{


    public CardData cardData;

    [Header("Card UI References")]
    private Image cardBackground;     // фон карты
    private Image cardImage;          // арт карты
    private TextMeshProUGUI nameText;
    private TextMeshProUGUI descriptionText;
    private TextMeshProUGUI attackText;
    private TextMeshProUGUI healthText;

    [Header("Extra UI")]
    public GameObject actionButton;  // кнопка, появляющаяся при нажатии

    [Header("Drag Settings")]
    public bool returnToStartPosition = true;
    public float dragSpeed = 1f;

    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private Vector2 startPosition;
    private Transform startParent;


    private bool isAnimated = false;
    private bool isClicked = false;


    // ----------------------------------------------
    // INIT
    // ----------------------------------------------

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    void Start()
    {
        canvas = GetComponentInParent<Canvas>();

        if (actionButton != null)
            actionButton.SetActive(false);

        if (cardData != null)
            UpdateCardUI();
    }

    // ----------------------------------------------
    // SET DATA
    // ----------------------------------------------

    public void InitializeCard(CardData data)
    {
        cardData = data;
        UpdateCardUI();
    }

    private void UpdateCardUI()
    {
        if (cardData == null) return;

        if (nameText) nameText.text = cardData.cardName;
        if (descriptionText) descriptionText.text = cardData.description;

        if (attackText)
            attackText.text = cardData.cardType == CardType.Creature ? cardData.attack.ToString() : string.Empty;

        if (healthText)
            healthText.text = cardData.cardType == CardType.Creature ? cardData.health.ToString() : string.Empty;

        if (cardImage && cardData.cardArt)
            cardImage.sprite = cardData.cardArt;

        if (cardBackground && cardData.cardBackground)
            gameObject.GetComponent<Image>().sprite = cardData.cardBackground;

        if (cardBackground)
            cardBackground.color = cardData.tintColor;
    }

    // ----------------------------------------------
    // DRAGGING
    // ----------------------------------------------

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = rectTransform.anchoredPosition;
        startParent = transform.parent;

        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.7f;

        transform.SetParent(canvas.transform, true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (canvas == null)
        {
            rectTransform.anchoredPosition += eventData.delta * dragSpeed;
            return;
        }

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            canvas.worldCamera,
            out Vector2 pos
        );

        rectTransform.position = canvas.transform.TransformPoint(pos);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        if (returnToStartPosition && transform.parent == canvas.transform)
        {
            rectTransform.anchoredPosition = startPosition;
            transform.SetParent(startParent, true);
        }
    }

    // ----------------------------------------------
    // HELPERS
    // ----------------------------------------------

    public void ResetPosition()
    {
        rectTransform.anchoredPosition = startPosition;
        transform.SetParent(startParent, true);
    }

    public void SetNewParent(Transform newParent)
    {
        startParent = newParent;
        transform.SetParent(newParent, true);
        rectTransform.anchoredPosition = Vector2.zero;
    }

    public CardData GetCardData() => cardData;

    // ----------------------------------------------
    // LOGIC
    // ----------------------------------------------

    //public virtual void oncardplayed()
    //{
    //    debug.log($"играна карта: {carddata.cardname}");

    //    switch (carddata.cardtype)
    //    {
    //        case cardtype.creature:
    //            debug.log("существо выходит на поле.");
    //            break;

    //        case cardtype.spell:
    //            debug.log("применено заклинание.");
    //            break;

    //        case cardtype.equipment:
    //            debug.log("экипировано снаряжение.");
    //            break;

    //        case cardtype.resource:
    //            debug.log("использован ресурс.");
    //            break;
    //    }
    //}

    // ----------------------------------------------
    // CLICK
    // ----------------------------------------------

    public float hoverScale = 1.1f;     // во сколько увеличить
    public float duration = 0.2f;       // длительность анимации
    public float delayBetween = 1f;   // минимальная пауза между срабатываниями

    private Tween currentTween;         // текущая анимация
    private float lastPlayTime = -Mathf.Infinity;

    public void OnPointerEnter(PointerEventData eventData)
    {

        if (Time.time - lastPlayTime < delayBetween)
            return;

        lastPlayTime = Time.fixedDeltaTime;

        Sequence seq = DOTween.Sequence();

        seq.Append(rectTransform.DORotate(new Vector3(0, 0, 4), 0.2f).SetEase(Ease.OutQuad));
        seq.Append(rectTransform.DOScale(1.1f, 0.2f).SetEase(Ease.OutQuad));
        seq.Append(rectTransform.DORotate(new Vector3(0, 0, -4), 0.2f).SetEase(Ease.OutQuad));
        seq.Append(rectTransform.DOScale(1.0f, 0.2f).SetEase(Ease.OutQuad));
        seq.Append(rectTransform.DORotate(Vector3.zero, 0.2f).SetEase(Ease.InQuad));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // можно оставить пустым или вернуть масштаб сразу
        // rectTransform.DOScale(1f, duration).SetEase(Ease.OutQuad);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isClicked == false && actionButton != null)
        {
            actionButton.SetActive(true);
            isClicked = true;
        }
        else if (isClicked == true && actionButton != null)
        {
            isClicked = false;
            actionButton.SetActive(false);
        }
    }
}
