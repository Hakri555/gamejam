using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Настройки перетаскивания")]
    public bool returnToStartPosition = true; // Возвращать карточку на исходную позицию
    public float dragSpeed = 1f; // Скорость перетаскивания

    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private Vector2 startPosition;
    private Transform startParent;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();

        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = rectTransform.anchoredPosition;
        startParent = transform.parent;

        canvasGroup.blocksRaycasts = false;

        canvasGroup.alpha = 0.7f;

        transform.SetParent(canvas.transform);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (canvas != null)
        {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                eventData.position,
                canvas.worldCamera,
                out position);

            rectTransform.position = canvas.transform.TransformPoint(position);
        }
        else
        {
            rectTransform.anchoredPosition += eventData.delta * dragSpeed;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;

        canvasGroup.blocksRaycasts = true;

        if (returnToStartPosition && transform.parent == canvas.transform)
        {
            rectTransform.anchoredPosition = startPosition;
            transform.SetParent(startParent);
        }
    }

    public void ResetPosition()
    {
        rectTransform.anchoredPosition = startPosition;
        transform.SetParent(startParent);
    }

    public void SetNewParent(Transform newParent)
    {
        startParent = newParent;
        transform.SetParent(newParent);
        rectTransform.anchoredPosition = Vector2.zero;
    }
}