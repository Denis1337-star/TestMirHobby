using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "UI Test/Character Definition")]
public class CharacterDefinition : ScriptableObject
{
    [Header("Info")]
    public string characterId;
    public string characterName;

    [Header("Portraits")]
    public Sprite partyPortrait;
    public Sprite bigPortrait;

    [Header("Stats")]
    public int hp;
    public int armor;

    [Header("Abilities")]
    public List<AbilityDefinition> abilities = new();

    [Header("Modifications")]
    public List<ModificationDefinition> modifications = new();
}
