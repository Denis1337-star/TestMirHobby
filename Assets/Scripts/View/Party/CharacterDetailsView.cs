using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// View правого/центрального блока с большой карточкой персонажа
/// Показывает портрет, имя и характеристики
/// </summary>
public class CharacterDetailsView : MonoBehaviour
{
    [SerializeField] private Image portrait;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text hpText;
    [SerializeField] private TMP_Text armorText;

    /// <summary>
    /// Отображает информацию по выбранному персонажу
    /// </summary>
    public void Show(CharacterDefinition def)
    {
        if (def == null)
            return;

        if (portrait != null)
            portrait.sprite = def.bigPortrait;

        if (nameText != null)
            nameText.text = def.characterName;

        if (hpText != null)
            hpText.text = $"{def.hp}/{def.hp}";

        if (armorText != null)
            armorText.text = $"{def.armor}/{ def.armor} ";
    }
}
