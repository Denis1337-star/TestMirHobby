using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "UI Test/Ability Definition")]
public class AbilityDefinition : ScriptableObject
{
    [Header("Base Info")]
    public string abilityId;
    public string abilityName;

    [Header("Visual")]
    public Sprite icon;

    [Header("Supported Modification Types")]
    public List<ModificationType> supportedTypes = new();
}

