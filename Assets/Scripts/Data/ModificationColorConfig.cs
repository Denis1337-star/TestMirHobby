using UnityEngine;

[CreateAssetMenu(menuName = "Game/Config/Modification Color Config")]
public class ModificationColorConfig : ScriptableObject
{
    public Color psykerColor;
    public Color dotColor;
    public Color attackColor;
    public Color buffColor;
    public Color debuffColor;

    public Color GetColor(ModificationType type)
    {
        switch (type)
        {
            case ModificationType.Psyker: return psykerColor;
            case ModificationType.Dot: return dotColor;
            case ModificationType.Attack: return attackColor;
            case ModificationType.Buff: return buffColor;
            case ModificationType.Debuff: return debuffColor;
            default: return Color.white;
        }
    }
}
