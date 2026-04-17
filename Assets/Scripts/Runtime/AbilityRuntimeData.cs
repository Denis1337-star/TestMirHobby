using System.Collections.Generic;


/// Runtime-данные ability
/// Хранит ссылку на definition и текущую привязанную модификацию

public class AbilityRuntimeData 
{
    public AbilityDefinition definition;

    public ModificationRuntimeData attachedModification;

    public AbilityRuntimeData(AbilityDefinition def)
    {
        definition = def;
    }


    /// Проверяет, поддерживает ли ability модификатор данного типа

    public bool Supports(ModificationType type)
    {
        return definition.supportedTypes != null &&
               definition.supportedTypes.Contains(type);
    }
}
