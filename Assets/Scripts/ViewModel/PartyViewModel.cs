using System;
using System.Collections.Generic;

/// ViewModel экрана party
/// Не знает ничего про Unity UI
/// Хранит текущее состояние экрана и бизнес-логику
/// выбор персонажа, назначение/снятие модификаций, выдача списков для отображения
public class PartyViewModel 
{
    private readonly List<CharacterRuntimeData> characters;
    private int selectedIndex = -1;
    public IReadOnlyList<CharacterRuntimeData> Characters => characters;   // Список всех персонажей
    public int SelectedIndex => selectedIndex;  // Индекс текущего выбранного персонажа
    public CharacterRuntimeData CurrentCharacter =>      // Текущий выбранный персонаж
        selectedIndex >= 0 && selectedIndex < characters.Count
            ? characters[selectedIndex]
            : null;

    public event Action OnCharacterChanged;  // Событие, когда сменился выбранный персонаж
    public event Action OnDataChanged;   // Событие, когда изменились данные текущего персонажа

    public PartyViewModel(List<CharacterRuntimeData> characters)
    {
        this.characters = characters ?? new List<CharacterRuntimeData>();
    }

    /// Выбирает персонажа по индексу

    public void SelectCharacter(int index)
    {
        if (index < 0 || index >= characters.Count)
            return;

        if (selectedIndex == index)
            return;

        selectedIndex = index;

        OnCharacterChanged?.Invoke();
        OnDataChanged?.Invoke();
    }

    /// Назначает модификацию на ability.
    /// Если у ability уже был модификатор — снимает его.
    /// Если модификатор был на другой ability — тоже переносит корректно.
    public void AssignModification(AbilityRuntimeData ability, ModificationRuntimeData mod)
    {
        if (ability == null || mod == null)
            return;

        if (ability.definition == null || mod.definition == null)
            return;

        if (!ability.Supports(mod.definition.type))
            return;

        // Если модификатор уже на другой способности — снимаем
        if (mod.attachedAbility != null)
            mod.attachedAbility.attachedModification = null;

        // Если на способности уже был другой модификатор — снимаем
        if (ability.attachedModification != null)
            ability.attachedModification.attachedAbility = null;

        ability.attachedModification = mod;
        mod.attachedAbility = ability;

        OnDataChanged?.Invoke();
    }

    /// <summary>
    /// Снимает модификацию с ability.
    /// </summary>
    public void RemoveModification(AbilityRuntimeData ability)
    {
        if (ability == null || ability.attachedModification == null)
            return;

        ModificationRuntimeData mod = ability.attachedModification;

        ability.attachedModification = null;
        mod.attachedAbility = null;

        OnDataChanged?.Invoke();
    }

    /// <summary>
    /// Возвращает список abilities текущего персонажа.
    /// </summary>
    public List<AbilityRuntimeData> GetAbilities()
    {
        return CurrentCharacter?.abilities;
    }

    /// <summary>
    /// Возвращает только свободные модификации текущего персонажа.
    /// Уже назначенные модификации в список не попадают.
    /// </summary>
    public List<ModificationRuntimeData> GetAvailableModifications()
    {
        if (CurrentCharacter == null)
            return null;

        List<ModificationRuntimeData> result = new();

        foreach (var mod in CurrentCharacter.modifications)
        {
            if (mod != null && !mod.IsAssigned)
                result.Add(mod);
        }

        return result;
    }
}
