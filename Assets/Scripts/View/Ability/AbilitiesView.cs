using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


/// <summary>
/// View списка abilities
/// Создает ability slots и управляет их подсветкой
/// </summary>
public class AbilitiesView : MonoBehaviour
{
    [SerializeField] private Transform content;
    [SerializeField] private AbilitySlotView itemPrefab;

    private readonly List<AbilitySlotView> spawnedItems = new();

    /// <summary>
    /// Полностью перестраивает UI списка abilities
    /// </summary>
    public void Show(
        IReadOnlyList<AbilityRuntimeData> abilities,
        Action<AbilityRuntimeData> hoverCallback,
        Action hoverExitCallback,
        Action<AbilityRuntimeData, PointerEventData> dropCallback,
        Action<AbilityRuntimeData> removeCallback)
    {
        Clear();

        if (abilities == null)
            return;

        for (int i = 0; i < abilities.Count; i++)
        {
            AbilitySlotView item = Instantiate(itemPrefab, content);

            item.Show(
                abilities[i],
                hoverCallback,
                hoverExitCallback,
                dropCallback,
                removeCallback);

            spawnedItems.Add(item);
        }
    }

    /// <summary>
    /// Подсвечивает только те abilities, которые совместимы с переданным модификатором
    /// </summary>
    public void HighlightCompatible(ModificationRuntimeData modification)
    {
        if (modification == null || modification.definition == null)
            return;

        foreach (var item in spawnedItems)
        {
            if (item == null || item.CurrentAbility == null)
                continue;

            bool isCompatible = item.CurrentAbility.Supports(modification.definition.type);
            item.SetHighlight(isCompatible);
        }
    }

    /// <summary>
    /// Сбрасывает подсветку у всех abilities
    /// </summary>
    public void ResetHighlight()
    {
        foreach (var item in spawnedItems)
        {
            if (item != null)
                item.SetHighlight(false);
        }
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
}
