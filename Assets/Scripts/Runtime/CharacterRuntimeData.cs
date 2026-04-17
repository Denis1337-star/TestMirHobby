using System.Collections;
using System.Collections.Generic;

/// Runtime-данные персонажа
/// На основе CharacterDefinition создает отдельные runtime-экземпляры
/// abilities и modifications для работы UI
public class CharacterRuntimeData
{
    public CharacterDefinition definition;

    public List<AbilityRuntimeData> abilities = new();
    public List<ModificationRuntimeData> modifications = new();

    public CharacterRuntimeData(CharacterDefinition definition)
    {
        this.definition = definition;

        if (definition == null)
            return;

        foreach (var abilityDefinition in definition.abilities)
        {
            if (abilityDefinition != null)
                abilities.Add(new AbilityRuntimeData(abilityDefinition));
        }

        foreach (var modificationDefinition in definition.modifications)
        {
            if (modificationDefinition != null)
                modifications.Add(new ModificationRuntimeData(modificationDefinition));
        }
    }
}
