using System;
using UnityEngine;
using UnityEngine.UI;

public class PartySlotView : MonoBehaviour
{
    /// <summary>
    /// View одного слота персонажа в party panel
    /// Отвечает только за отображение аватара и selection frame
    /// </summary>


    [SerializeField] private Image portrait;
    [SerializeField] private GameObject selectedFrame;
    [SerializeField] private Button button;

    private int index;
    private Action<int> onClick;

    /// <summary>
    /// Инициализирует слот
    /// </summary>
    public void Setup(int index, Sprite icon, Action<int> clickCallback)
    {
        this.index = index;
        onClick = clickCallback;

        if (portrait != null)
            portrait.sprite = icon;

        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(HandleClick);
        }
    }

    /// <summary>
    /// Включает / выключает рамку выбранного персонажа
    /// </summary>
    public void SetSelected(bool value)
    {
        if (selectedFrame != null)
            selectedFrame.SetActive(value);
    }

    private void HandleClick()
    {
        onClick?.Invoke(index);
    }
}

