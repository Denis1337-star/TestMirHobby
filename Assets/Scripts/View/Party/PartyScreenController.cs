using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Главный binder экрана party
/// Создает ViewModel, подписывается на ее события
/// и перенаправляет пользовательские действия из View в ViewModel
/// </summary>
public class PartyScreenController : MonoBehaviour
{
    [Header("Configs")]
    [SerializeField] private List<CharacterDefinition> characterDefinitions;

    [Header("Views")]
    [SerializeField] private PartyPanelView partyPanelView;
    [SerializeField] private CharacterDetailsView characterDetailsView;
    [SerializeField] private AbilitiesView abilitiesView;
    [SerializeField] private ModificationsView modificationsView;

    private PartyViewModel viewModel;

    // Флаг, что сейчас идет drag модификации.
    // Нужен, чтобы hover-события не сбрасывали подсветку во время перетаскивания
    private bool isDraggingModification;

    // Ссылка на текущую перетаскиваемую модификацию
    private ModificationRuntimeData currentDraggedModification;

    private void Start()
    {
        List<CharacterRuntimeData> runtimeCharacters = new();

        foreach (var definition in characterDefinitions)
        {
            if (definition != null)
                runtimeCharacters.Add(new CharacterRuntimeData(definition));
        }

        viewModel = new PartyViewModel(runtimeCharacters);

        // Подписываемся на изменения состояния ViewModel
        viewModel.OnCharacterChanged += RefreshCharacter;
        viewModel.OnCharacterChanged += RefreshPartySelection;
        viewModel.OnDataChanged += RefreshAll;

        // Сразу строим panel со слотами персонажей
        partyPanelView.Show(
            viewModel.Characters,
            viewModel.SelectedIndex,
            OnCharacterSelected);

        // По умолчанию выбираем первого персонажа
        if (viewModel.Characters.Count > 0)
            viewModel.SelectCharacter(0);
    }

    private void OnDestroy()
    {
        if (viewModel == null)
            return;

        viewModel.OnCharacterChanged -= RefreshCharacter;
        viewModel.OnCharacterChanged -= RefreshPartySelection;
        viewModel.OnDataChanged -= RefreshAll;
    }

    /// <summary>
    /// Вызывается, когда пользователь кликнул по аватару персонажа
    /// </summary>
    private void OnCharacterSelected(int index)
    {
        viewModel.SelectCharacter(index);
    }

    /// <summary>
    /// Обновляет selection frame в панели персонажей
    /// </summary>
    private void RefreshPartySelection()
    {
        partyPanelView.UpdateSelection(viewModel.SelectedIndex);
    }

    /// <summary>
    /// Обновляет блок деталей выбранного персонажа
    /// </summary>
    private void RefreshCharacter()
    {
        CharacterRuntimeData character = viewModel.CurrentCharacter;
        if (character == null)
            return;

        characterDetailsView.Show(character.definition);
    }

    /// <summary>
    /// Полностью обновляет abilities и modifications текущего персонажа
    /// </summary>
    private void RefreshAll()
    {
        // Если UI перестраивается во время drag
        modificationsView.HideDragPreview();

        List<AbilityRuntimeData> abilities = viewModel.GetAbilities();
        List<ModificationRuntimeData> modifications = viewModel.GetAvailableModifications();

        abilitiesView.Show(
            abilities,
            OnAbilityHovered,
            OnHoverExit,
            OnAbilityDropped,
            OnRemoveModification);

        modificationsView.Show(
            modifications,
            OnModificationHovered,
            OnHoverExit,
            OnModificationBeginDrag,
            OnModificationEndDrag);

        // Если вдруг UI перестроился во время drag
        // восстанавливаем подсветку подходящих abilities
        if (isDraggingModification && currentDraggedModification != null)
            abilitiesView.HighlightCompatible(currentDraggedModification);
    }

    /// <summary>
    /// Обработка успешного drop модификации на ability
    /// </summary>
    private void OnAbilityDropped(AbilityRuntimeData ability, PointerEventData eventData)
    {
        ModificationSlotView draggedSlot = eventData.pointerDrag?.GetComponent<ModificationSlotView>();
        if (draggedSlot == null || draggedSlot.CurrentModification == null)
            return;

        viewModel.AssignModification(ability, draggedSlot.CurrentModification);

        isDraggingModification = false;
        currentDraggedModification = null;

        abilitiesView.ResetHighlight();
        modificationsView.ResetHighlight();
        modificationsView.HideDragPreview();
    }

    /// <summary>
    /// Снимает модификацию с ability по ПКМ
    /// </summary>
    private void OnRemoveModification(AbilityRuntimeData ability)
    {
        viewModel.RemoveModification(ability);

        abilitiesView.ResetHighlight();
        modificationsView.ResetHighlight();
        modificationsView.HideDragPreview();
    }

    /// <summary>
    /// Hover по ability:
    /// подсвечиваем совместимые модификации
    /// Но только если сейчас не идет drag
    /// </summary>
    private void OnAbilityHovered(AbilityRuntimeData ability)
    {
        if (isDraggingModification)
            return;

        modificationsView.HighlightCompatible(ability);
    }

    /// <summary>
    /// Hover по модификации:
    /// подсвечиваем совместимые abilities
    /// Но только если сейчас не идет drag
    /// </summary>
    private void OnModificationHovered(ModificationRuntimeData modification)
    {
        if (isDraggingModification)
            return;

        abilitiesView.HighlightCompatible(modification);
    }

    /// <summary>
    /// Общий выход из hover
    /// Во время drag подсветку не сбрасываем
    /// </summary>
    private void OnHoverExit()
    {
        if (isDraggingModification)
            return;

        abilitiesView.ResetHighlight();
        modificationsView.ResetHighlight();
    }

    /// <summary>
    /// Начало drag модификации
    /// В этот момент фиксируем drag-состояние
    /// и подсвечиваем все совместимые abilities
    /// </summary>
    private void OnModificationBeginDrag(ModificationRuntimeData modification)
    {
        isDraggingModification = true;
        currentDraggedModification = modification;

        abilitiesView.HighlightCompatible(modification);
    }

    /// <summary>
    /// Конец drag модификации
    /// Сбрасываем drag-state и убираем подсветку
    /// </summary>
    private void OnModificationEndDrag()
    {
        isDraggingModification = false;
        currentDraggedModification = null;

        abilitiesView.ResetHighlight();
        modificationsView.HideDragPreview();
    }
}
