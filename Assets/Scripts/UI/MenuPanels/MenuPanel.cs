using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

abstract public class MenuPanel : MonoBehaviour
{
    private const float Space = 5f;

    private List<MenuButton> buttons = new List<MenuButton>();
    private MenuButton currentButtonSelected;

    public MenuPanel ParentMenuPanel { get; set; }
    public List<Transform> PanelElements { get; } = new List<Transform>();

    public abstract void CollectPanelElementsToList();

    public virtual void Awake()
    {
        CollectPanelElementsToList();
        PackButtons();
    }

    private void PackButtons()
    {
        for (int i = 0; i < PanelElements.Count; i++)
        {
            var button = PanelElements[i].GetComponent<MenuButton>();
            if (button != null)
            {
                buttons.Add(button);
                button.SelectButton(false);
            }
        }
    }

    private void EnableButtonsSelectSubscribe(bool active)
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            if (active)
                buttons[i].OnSelected += SetSelectedButton;
            else
                buttons[i].OnSelected -= SetSelectedButton;
        }
    }

    private void CalculateMenuPanelPosition(Transform button)
    {
        var transf = button.parent.transform;
        var vlg = transf.GetComponent<VerticalLayoutGroup>();
        var rectTransform = ParentMenuPanel.GetComponent<RectTransform>();
        var buttonRT = button.GetComponent<RectTransform>();
        var width = rectTransform.rect.width;
        var height = GetComponent<RectTransform>().rect.height;
        transform.position = new Vector3(button.position.x + width + Space, button.position.y + (height / 2 - buttonRT.rect.height * 2 + vlg.spacing / 2));
    }

    public void Open(MenuPanel parentPanel, Transform buttonOfOpen)
    {
        if (ParentMenuPanel != parentPanel)
            ParentMenuPanel = parentPanel;

        SetActiveParentPanelButtons(false);
        
        gameObject.SetActive(true);
        RotateElements();

        if (buttons.Count > 0)
        {
            buttons[0].Select();
            buttons[0].OnSelect(null);
        }

        if (buttonOfOpen != null)
            CalculateMenuPanelPosition(buttonOfOpen);
        EnableButtonsSelectSubscribe(true);
        AnimateButtons();
    }

    public void Close()
    {
        EnableButtonsSelectSubscribe(false);
        ParentMenuPanel?.OnReturn();
        SetActiveParentPanelButtons(true);
        gameObject.SetActive(false);
    }

    private void SetActiveParentPanelButtons(bool active)
    {
        if (ParentMenuPanel != null)
        {
            for (int i = 0; i < ParentMenuPanel.PanelElements.Count; i++)
            {
                var element = ParentMenuPanel.PanelElements[i];
                var menuButton = element.GetComponent<MenuButton>();
                if(menuButton != null)
                    menuButton.IsInteractable = active;
            }
        }
    }

    public void SetSelectedButton(MenuButton newSelectedButton)
    {
        if (currentButtonSelected != null)
        {
            if (currentButtonSelected == newSelectedButton)
                return;

            currentButtonSelected.SelectButton(false);
        }

        currentButtonSelected = newSelectedButton;
        currentButtonSelected.SelectButton(true);
    }

    private void RotateElements()
    {
        for (int i = 0; i < PanelElements.Count; i++)
        {
            PanelElements[i].transform.rotation = Quaternion.Euler(Vector3.up * 90);
        }
    }

    public virtual void OnReturn()
    {
        if (currentButtonSelected != null)
            currentButtonSelected.Select();
    }

    protected void AnimateButtons()
    {
        Debug.LogWarning($"Animate buttons {PanelElements.Count}");
        StartCoroutine(CorAnimateButtons(PanelElements));
    }

    private IEnumerator CorAnimateButtons(List<Transform> elements)
    {
        for (int i = 0; i < elements.Count; i++)
        {
            var button = elements[i];
            Sequence buttonSequence = DOTween.Sequence();
            buttonSequence.Append(button.DORotate(Vector3.up * -10f, 0.5f));
            buttonSequence.Append(button.DORotate(Vector3.zero, 0.5f));
            buttonSequence.Play().OnKill(() => buttonSequence = null);
            yield return new WaitForSeconds(0.05f);
        }
    }
}
