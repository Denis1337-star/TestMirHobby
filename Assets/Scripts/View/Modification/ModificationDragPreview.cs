using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Визуальный drag-preview модификации
/// Показывается во время перетаскивания
/// </summary>
public class ModificationDragPreview : MonoBehaviour
{
    [SerializeField] private Image padImage;
    [SerializeField] private Image iconImage;
    [SerializeField] private ModificationColorConfig colorConfig;

    /// <summary>
    /// Показывает drag preview на основе переданного definition
    /// </summary>
    public void Show(ModificationDefinition definition)
    {
        if (definition == null)
            return;

        if (padImage != null && colorConfig != null)
            padImage.color = colorConfig.GetColor(definition.type);

        if (iconImage != null)
            iconImage.sprite = definition.icon;

        gameObject.SetActive(true);
        transform.SetAsLastSibling();
    }

    /// <summary>
    /// Скрывает drag preview
    /// </summary>
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Перемещает preview в позицию курсора
    /// </summary>
    public void SetScreenPosition(Canvas canvas, Vector2 screenPosition, Camera eventCamera)
    {
        if (canvas == null)
            return;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            screenPosition,
            eventCamera,
            out var localPosition);

        ((RectTransform)transform).localPosition = localPosition;
    }
}
