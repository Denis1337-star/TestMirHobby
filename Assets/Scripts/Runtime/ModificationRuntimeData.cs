
/// Runtime-данные модификации
/// Хранит ссылку на definition и ability, к которой модификация сейчас привязана
public class ModificationRuntimeData 
{
    public ModificationDefinition definition;

    public AbilityRuntimeData attachedAbility;

    public bool IsAssigned => attachedAbility != null;

    /// True, если модификация уже назначена на какую-то ability
    public ModificationRuntimeData(ModificationDefinition def)
    {
        definition = def;
    }
}
