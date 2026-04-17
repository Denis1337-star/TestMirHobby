using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// View одной модификации в списке
/// Отвечает за hover, drag begin/drag/end и свой визуал
/// </summary>
public class ModificationSlotView : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler
{
    [Header("UI")]
    [SerializeField] private Image padIconImage;
    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private GameObject highlightObject;

    [SerializeField] private ModificationColorConfig colorConfig;

    private ModificationRuntimeData currentModification;

    private Action<ModificationRuntimeData> onHover;
    private Action onHoverExit;
    private Action<ModificationRuntimeData> onBeginDrag;
    private Action onEndDrag;

    private Canvas rootCanvas;
    private CanvasGroup canvasGroup;
    private ModificationDragPreview dragPreview;

    private bool isDragging;

    /// <summary>
    /// Runtime-данные модификации, показанной в этом слоте
    /// </summary>
    public ModificationRuntimeData CurrentModification => currentModification;

    /// <summary>
    /// Передает runtime-зависимости для drag-системы
    /// </summary>
    public void SetupRuntime(Canvas canvas, CanvasGroup group, ModificationDragPreview preview)
    {
        rootCanvas = canvas;
        canvasGroup = group;
        dragPreview = preview;
    }

    /// <summary>
    /// Заполняет слот данными.
    /// </summary>
    public void Show(
        ModificationRuntimeData modification,
        Action<ModificationRuntimeData> hoverCallback,
        Action hoverExitCallback,
        Action<ModificationRuntimeData> beginDragCallback,
        Action endDragCallback)
    {
        currentModification = modification;
        onHover = hoverCallback;
        onHoverExit = hoverExitCallback;
        onBeginDrag = beginDragCallback;
        onEndDrag = endDragCallback;

        if (modification == null || modification.definition == null)
            return;

        if (padIconImage != null && colorConfig != null)
            padIconImage.color = colorConfig.GetColor(modification.definition.type);

        if (iconImage != null)
            iconImage.sprite = modification.definition.icon;

        if (nameText != null)
            nameText.text = modification.definition.modificationName;

        if (descriptionText != null)
            descriptionText.text = modification.definition.type.ToString();

        SetHighlight(false);
    }

    /// <summary>
    /// Включает / выключает highlight-объект
    /// </summary>
    public void SetHighlight(bool value)
    {
        if (highlightObject != null)
            highlightObject.SetActive(value);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isDragging && currentModification != null)
            onHover?.Invoke(currentModification);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isDragging)
            onHoverExit?.Invoke();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (currentModification == null || currentModification.definition == null)
            return;

        if (rootCanvas == null || canvasGroup == null || dragPreview == null)
            return;

        isDragging = true;

        // Чтобы во время drag этот UI-элемент не блокировал raycast под собой
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.3f;

        dragPreview.Show(currentModification.definition);
        dragPreview.SetScreenPosition(rootCanvas, eventData.position, eventData.pressEventCamera);

        onBeginDrag?.Invoke(currentModification);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging || dragPreview == null || rootCanvas == null)
            return;

        dragPreview.SetScreenPosition(rootCanvas, eventData.position, eventData.pressEventCamera);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;

        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1f;
        }

        if (dragPreview != null)
            dragPreview.Hide();

        onEndDrag?.Invoke();
    }
}
