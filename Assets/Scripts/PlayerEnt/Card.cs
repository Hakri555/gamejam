using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;





public class DraggableCard : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler,
    IPointerEnterHandler, IPointerClickHandler
{
    [Header("Resouces menu")]
    public GameObject CardManager;
    private CardManager _cardManager;

    [Header("Card Dictionary")]

    public List<string> data;
    public List<int> col;
    
    [Header("Card Data")]
    public CardData cardData;
    public GameObject actionButton;
    public GameObject foreGround;

    [Header("Drag Settings")]
    public float dragSpeed = 1f;

    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;

    private Vector2 startPosition;
    private int startSiblingIndex;
    private Transform startParent;

    private LayoutGroup parentLayoutGroup;
    private ContentSizeFitter parentSizeFitter;

    private GameObject prefab;
    private bool isClicked = false;

    public bool isTurret = false;
    public bool coudDrag = false;
    public bool gameStarted = false;

    void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        _cardManager = CardManager.GetComponent<CardManager>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        actionButton.SetActive(false);
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        startParent = transform.parent;   // но позицию НЕ сохраняем здесь
        prefab = cardData != null ? cardData.prefab : null;
    }

    void Start()
    {
        startParent = transform.parent;
        startSiblingIndex = transform.GetSiblingIndex();
        startPosition = rectTransform.anchoredPosition;
        foreGround.GetComponent<Image>().sprite = cardData.cardImage;
        parentLayoutGroup = startParent.GetComponent<LayoutGroup>();
        parentSizeFitter = startParent.GetComponent<ContentSizeFitter>();
        WaveManager.OnEventStarted += TurnOfCard;
    
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        int i = 0;
        foreach(var str in data) 
        {
            switch (str) 
            {
                case "iron":
                    if (_cardManager.ironN > col[i]) 
                    {
                        coudDrag = true;
                    }
                    break;
                case "steel":
                    if (_cardManager.steelN > col[i])
                    {
                        coudDrag = true;
                    }
                    break;
                case "copper":
                    if (_cardManager.copperN > col[i])
                    {
                        coudDrag = true;
                    }
                    break;
                case "adam":
                    if (_cardManager.AdamantitN > col[i])
                    {
                        coudDrag = true;
                    }
                    break;
            }
        }
        if (!coudDrag || (gameStarted == true))
            return;
        canvasGroup.alpha = 0.7f;
        canvasGroup.blocksRaycasts = false;

        // Отключаем layout, чтобы он не дергал карточку во время перетаскивания
        if (parentLayoutGroup != null) parentLayoutGroup.enabled = false;
        if (parentSizeFitter != null) parentSizeFitter.enabled = false;

        // Переносим карточку на Canvas (с сохранением мировой позиции)
        transform.SetParent(canvas.transform, true);
    }



    public void OnDrag(PointerEventData eventData)
    {
        if (!coudDrag || (gameStarted == true))
            return;
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

        GameObject nearest = null;
       
        
        float minDist = float.MaxValue;

        // Ищем ближайший DropSlot
        if (!isTurret)
        {
            foreach (DropSlot slot in FindObjectsOfType<DropSlot>())
            {
                float dist = Vector2.Distance(rectTransform.position, slot.transform.position);
                if (dist < minDist && dist < 1f)
                {
                    minDist = dist;
                    nearest = slot.gameObject;
                }
            }
        }
        else 
        {

            foreach (DropSlotTower slot in FindObjectsOfType<DropSlotTower>())
            {
                float dist = Vector2.Distance(rectTransform.position, slot.transform.position);
                if (dist < minDist && dist < 1f)
                {
                    minDist = dist;
                    nearest = slot.gameObject;
                }
            }
        }
        if (nearest != null)
        {
            int i = 0;
            foreach (var str in data)
            {

                switch (str)
                {
                    case "iron":
                        if (_cardManager.ironN > col[i])
                        {
                            _cardManager.SetIronN(_cardManager.data.iron - col[i]);
                            Debug.Log("хочу железа");
                        }
                        break;
                    case "steel":
                        if (_cardManager.steelN > col[i])
                        {
                            _cardManager.SetSteelN(_cardManager.data.steel - col[i]);
                        }
                        break;
                    case "copper":
                        if (_cardManager.copperN > col[i])
                        {
                            _cardManager.SetCopperN(_cardManager.data.copper - col[i]);
                        }
                        break;
                    case "adam":
                        if (_cardManager.AdamantitN > col[i])
                        {
                            _cardManager.SetAdamantitN(_cardManager.data.adamantium - col[i]);

                        }
                        break;
                }
            }
            if (prefab != null && (nearest.transform.childCount == 0))
                Instantiate(prefab, nearest.transform.position, Quaternion.identity,nearest.transform);



        }

        ReturnToHand();
    }


    private void ReturnToHand()
    {
        if (startParent == null) return;

        Vector3 worldBefore = rectTransform.position;

        if (parentLayoutGroup != null) parentLayoutGroup.enabled = false;
        if (parentSizeFitter != null) parentSizeFitter.enabled = false;

        // возвращаем в руку
        transform.SetParent(startParent, true);

        // ВОССТАНАВЛИВАЕМ ИСХОДНЫЙ ПОРЯДОК
        transform.SetSiblingIndex(startSiblingIndex);

        LayoutRebuilder.ForceRebuildLayoutImmediate(startParent as RectTransform);

        Vector3 worldTarget = rectTransform.position;
        rectTransform.position = worldBefore;

        // анимация возврата
        rectTransform.DOMove(worldTarget, 0.25f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                if (parentLayoutGroup != null) parentLayoutGroup.enabled = true;
                if (parentSizeFitter != null) parentSizeFitter.enabled = true;

                LayoutRebuilder.ForceRebuildLayoutImmediate(startParent as RectTransform);
            });
    }



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



    public void OnPointerClick(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        if (isClicked == false )
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

    private void TurnOfCard() 
    {
        if (gameStarted == true) 
        {
            gameStarted = false;
        }
        else
        {
            gameStarted = true;
        }
    }
   
}
