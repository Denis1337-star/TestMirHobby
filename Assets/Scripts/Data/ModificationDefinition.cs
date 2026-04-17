using UnityEngine;

// [System.Flags] нужен для того,
// чтобы одна Ability могла поддерживать сразу несколько типов
[System.Flags]
public enum ModificationType
{
    None = 0,
    Psyker = 1 << 0,
    Dot = 1 << 1,
    Attack = 1 << 2,
    Debuff = 1 << 3,
    Buff = 1 << 4
}

[CreateAssetMenu(menuName = "UI Test/Modification Definition")]
public class ModificationDefinition : ScriptableObject
{
    [Header("Base Info")]
    public string modificationId;
    public string modificationName;

    [Header("Type")]
    public ModificationType type;

    [Header("Visual")]
    public Sprite icon;
}