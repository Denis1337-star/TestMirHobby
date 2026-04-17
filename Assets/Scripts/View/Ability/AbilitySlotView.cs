using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// View одной ability
/// Отвечает за отображение ability, подсветку, drop модификации
/// и визуальное состояние назначенного модификатора
/// </summary>
public class AbilitySlotView : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IDropHandler,
    IPointerClickHandler
{
    [Header("Root")]
    [SerializeField] private GameObject root;

    [Header("Base UI")]
    [SerializeField] private Image abilityIconImage;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private GameObject highlightObject;

    [Header("Union Visual")]
    [SerializeField] private Image unionImage;
    [SerializeField] private Outline unionOutline;

    [Header("Decor")]
    [SerializeField] private Image decorTop;
    [SerializeField] private Image decorBottom;

    [Header("Assigned Mod UI")]
    [SerializeField] private Image modIconImage;

    [Header("Colors")]
    [SerializeField] private Color defaultUnionColor = Color.white;
    [SerializeField] private Color defaultOutlineColor = Color.white;
    [SerializeField] private Color activeOutlineColor = Color.yellow;

    [SerializeField] private ModificationColorConfig colorConfig;

    private AbilityRuntimeData ability;

    private Action<AbilityRuntimeData> onHover;
    private Action onHoverExit;
    private Action<AbilityRuntimeData, PointerEventData> onDrop;
    private Action<AbilityRuntimeData> onRemove;

    /// <summary>
    /// Runtime-данные текущей ability, показанной в этом слоте
    /// </summary>
    public AbilityRuntimeData CurrentAbility => ability;

    /// <summary>
    /// Заполняет слот данными и подписывает внешние callbacks
    /// </summary>
    public void Show(
        AbilityRuntimeData data,
        Action<AbilityRuntimeData> hoverCallback,
        Action hoverExitCallback,
        Action<AbilityRuntimeData, PointerEventData> dropCallback,
        Action<AbilityRuntimeData> removeCallback)
    {
        ability = data;
        onHover = hoverCallback;
        onHoverExit = hoverExitCallback;
        onDrop = dropCallback;
        onRemove = removeCallback;

        if (data == null || data.definition == null)
        {
            Hide();
            return;
        }

        (root ?? gameObject).SetActive(true);

        if (abilityIconImage != null)
            abilityIconImage.sprite = data.definition.icon;

        if (nameText != null)
            nameText.text = data.definition.abilityName;

        RefreshAssignedVisual();
        SetHighlight(false);
    }

    /// <summary>
    /// Скрывает слот, если данные отсутствуют
    /// </summary>
    public void Hide()
    {
        ability = null;
        (root ?? gameObject).SetActive(false);
    }

    /// <summary>
    /// Включает / выключает highlight-объект
    /// </summary>
    public void SetHighlight(bool value)
    {
        if (highlightObject != null)
            highlightObject.SetActive(value);
    }

    /// <summary>
    /// Обновляет визуальное состояние ability
    /// если на ней есть привязанный модификатор
    /// </summary>
    public void RefreshAssignedVisual()
    {
        if (ability == null)
            return;

        bool hasModification = ability.attachedModification != null;

        if (!hasModification)
        {
            if (unionImage != null)
                unionImage.color = defaultUnionColor;

            if (unionOutline != null)
                unionOutline.effectColor = defaultOutlineColor;

            if (decorTop != null)
                decorTop.color = defaultOutlineColor;

            if (decorBottom != null)
                decorBottom.color = defaultOutlineColor;

            if (modIconImage != null)
                modIconImage.gameObject.SetActive(false);

            return;
        }

        ModificationRuntimeData modification = ability.attachedModification;

        Color fillColor = colorConfig != null
            ? colorConfig.GetColor(modification.definition.type)
            : Color.white;

        if (unionImage != null)
            unionImage.color = fillColor;

        if (unionOutline != null)
            unionOutline.effectColor = activeOutlineColor;

        if (decorTop != null)
            decorTop.color = activeOutlineColor;

        if (decorBottom != null)
            decorBottom.color = activeOutlineColor;

        if (modIconImage != null)
        {
            modIconImage.gameObject.SetActive(true);
            modIconImage.sprite = modification.definition.icon;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (ability == null)
            return;

        onHover?.Invoke(ability);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        onHoverExit?.Invoke();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (ability == null)
            return;

        onDrop?.Invoke(ability, eventData);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (ability == null)
            return;

        if (eventData.button == PointerEventData.InputButton.Right &&
            ability.attachedModification != null)
        {
            onRemove?.Invoke(ability);
        }
    }
}
