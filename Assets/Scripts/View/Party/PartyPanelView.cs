using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// View панели персонажей
/// —оздает все аватары автоматически и обновл€ет их состо€ние selection
/// </summary>
public class PartyPanelView : MonoBehaviour
{
    [SerializeField] private Transform content;
    [SerializeField] private PartySlotView slotPrefab;

    private readonly List<PartySlotView> spawnedSlots = new();

    /// <summary>
    /// ѕолностью перестраивает список персонажей
    /// </summary>
    public void Show(IReadOnlyList<CharacterRuntimeData> characters, int selectedIndex, Action<int> clickCallback)
    {
        Clear();

        if (characters == null)
            return;

        for (int i = 0; i < characters.Count; i++)
        {
            CharacterRuntimeData character = characters[i];
            if (character == null || character.definition == null)
                continue;

            PartySlotView slot = Instantiate(slotPrefab, content);
            slot.Setup(i, character.definition.partyPortrait, clickCallback);
            slot.SetSelected(i == selectedIndex);

            spawnedSlots.Add(slot);
        }
    }

    /// <summary>
    /// ќбновл€ет только визуальное состо€ние selectedFrame
    /// </summary>
    public void UpdateSelection(int selectedIndex)
    {
        for (int i = 0; i < spawnedSlots.Count; i++)
        {
            if (spawnedSlots[i] != null)
                spawnedSlots[i].SetSelected(i == selectedIndex);
        }
    }

    private void Clear()
    {
        foreach (var slot in spawnedSlots)
        {
            if (slot != null)
                Destroy(slot.gameObject);
        }

        spawnedSlots.Clear();
    }
}
