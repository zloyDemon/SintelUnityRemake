using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    [SerializeField] Image selector;
    [SerializeField] TextMeshProUGUI buttonText;
    [SerializeField] Button button;

    public string Text { get { return buttonText.text; } set { buttonText.text = value; } }
    public bool IsInteractable { set { button.interactable = value; } get { return button.interactable; } }
    public Button Button => button;

    public event Action<MenuButton> OnSelected = mb => { };

    private void Awake()
    {

    }

    public virtual void SetClickListener(UnityAction clickEvent)
    {
        button.onClick.AddListener(clickEvent);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!button.interactable)
            return;

        Select();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!button.interactable)
            return;
    }

    public void Select()
    {
        EventSystem.current.SetSelectedGameObject(button.gameObject);
    }

    private void SelectCursorActive(bool select)
    {
        selector.gameObject.SetActive(select);
    }

    public void SelectButton(bool select)
    {
        SelectCursorActive(select);
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (!button.interactable)
            return;

        SelectCursorActive(true);
        OnSelected(this);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (!button.interactable)
            return;

        SelectCursorActive(false);
    }
}
