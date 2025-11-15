using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;

public class DraggableCard : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler,
    IPointerEnterHandler, IPointerClickHandler

{
    [Header("Card Data")]
    public CardData cardData;
    public GameObject actionButton;

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


    void Awake()
    {
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

        parentLayoutGroup = startParent.GetComponent<LayoutGroup>();
        parentSizeFitter = startParent.GetComponent<ContentSizeFitter>();
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
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

        DropSlot nearest = null;
        float minDist = float.MaxValue;

        // Ищем ближайший DropSlot
        foreach (DropSlot slot in FindObjectsOfType<DropSlot>())
        {
            float dist = Vector2.Distance(rectTransform.position, slot.transform.position);
            if (dist < minDist && dist < 1f)
            {
                minDist = dist;
                nearest = slot;
            }
        }

        if (nearest != null)
        {
            // Создаём prefab в слоте
            if (prefab != null)
                Instantiate(prefab, nearest.transform.position, Quaternion.identity);
        }

        // В любом случае карта возвращается
        ReturnToHand();
    }


    private void ReturnToHand()
    {
        if (startParent == null) return;

        // 1. Сохраняем текущую мировую позицию
        Vector3 worldBefore = rectTransform.position;

        // 2. Отключаем layout на родителе
        if (parentLayoutGroup != null) parentLayoutGroup.enabled = false;
        if (parentSizeFitter != null) parentSizeFitter.enabled = false;

        // 3. Репарентим обратно в Content с сохранением мировой позиции
        transform.SetParent(startParent, true);

        // 4. Форсируем rebuild layout, чтобы получить target-позицию
        LayoutRebuilder.ForceRebuildLayoutImmediate(startParent as RectTransform);

        // 5. Целевая мировая позиция
        Vector3 worldTarget = rectTransform.position;

        // 6. Ставим обратно на визуальную позицию перед анимацией
        rectTransform.position = worldBefore;

        // 7. Анимируем в target
        rectTransform.DOMove(worldTarget, 0.25f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            // 8. Включаем layout обратно
            if (parentLayoutGroup != null) parentLayoutGroup.enabled = true;
            if (parentSizeFitter != null) parentSizeFitter.enabled = true;

            // 9. Фиксируем локальную позицию
            rectTransform.anchoredPosition = rectTransform.anchoredPosition;

            // Форсируем rebuild, чтобы layout переставил элементы
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
   
}
