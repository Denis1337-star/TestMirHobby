using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// View списка модификаций.
/// Создает modification slots, настраивает drag runtime
/// и управляет подсветкой
/// </summary>
public class ModificationsView : MonoBehaviour
{
    [SerializeField] private Transform content;
    [SerializeField] private ModificationSlotView itemPrefab;

    [Header("Runtime")]
    [SerializeField] private Canvas rootCanvas;
    [SerializeField] private ModificationDragPreview dragPreview;

    private readonly List<ModificationSlotView> spawnedItems = new();

    /// <summary>
    /// Полностью перестраивает UI списка доступных модификаций
    /// Уже назначенные модификаторы не отображаются
    /// </summary>
    public void Show(
        IReadOnlyList<ModificationRuntimeData> modifications,
        Action<ModificationRuntimeData> hoverCallback,
        Action hoverExitCallback,
        Action<ModificationRuntimeData> beginDragCallback,
        Action endDragCallback)
    {
        Clear();
        HideDragPreview();

        if (modifications == null)
            return;

        foreach (var modification in modifications)
        {
            if (modification == null || modification.IsAssigned)
                continue;

            ModificationSlotView item = Instantiate(itemPrefab, content);

            CanvasGroup group = item.GetComponent<CanvasGroup>();
            if (group == null)
                group = item.gameObject.AddComponent<CanvasGroup>();

            item.SetupRuntime(rootCanvas, group, dragPreview);

            item.Show(
                modification,
                hoverCallback,
                hoverExitCallback,
                beginDragCallback,
                endDragCallback);

            spawnedItems.Add(item);
        }

        RefreshLayout();
    }

    /// <summary>
    /// Подсвечивает только те модификации, которые подходят выбранной ability
    /// </summary>
    public void HighlightCompatible(AbilityRuntimeData ability)
    {
        if (ability == null)
            return;

        foreach (var item in spawnedItems)
        {
            if (item == null || item.CurrentModification == null)
                continue;

            bool isCompatible = ability.Supports(item.CurrentModification.definition.type);
            item.SetHighlight(isCompatible);
        }
    }

    /// <summary>
    /// Сбрасывает подсветку у всех модификаций
    /// </summary>
    public void ResetHighlight()
    {
        foreach (var item in spawnedItems)
        {
            if (item != null)
                item.SetHighlight(false);
        }
    }

    /// <summary>
    /// Принудительно скрывает drag preview
    /// Это полезно, если UI перестраивается во время drag
    /// </summary>
    public void HideDragPreview()
    {
        if (dragPreview != null)
            dragPreview.Hide();
    }

    private void Clear()
    {
        foreach (var item in spawnedItems)
        {
            if (item != null)
                Destroy(item.gameObject);
        }

        spawnedItems.Clear();
    }

    private void RefreshLayout()
    {
        if (content is RectTransform rectTransform)
            LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);

        Canvas.ForceUpdateCanvases();
    }
}
