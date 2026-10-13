using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

public class MenuButtonController : MonoBehaviour
{
    public List<RectTransform> buttons;
    public float focusScale = 1.3f;
    public float normalScale = 1f;
    public float animDuration = 0.4f;
    public Vector2 centerPosition;

    private RectTransform currentFocused;
    private Vector2[] originalPositions;

    void Start()
    {
        originalPositions = new Vector2[buttons.Count];
        for (int i = 0; i < buttons.Count; i++)
        {
            originalPositions[i] = buttons[i].anchoredPosition;
        }
    }

    public void FocusButton(RectTransform selectedButton)
    {
        if (currentFocused == selectedButton) return;

        foreach (RectTransform btn in buttons)
        {
            btn.DOScale(normalScale, animDuration);
        }

        if (currentFocused != null)
        {
            int idx = buttons.IndexOf(currentFocused);
            currentFocused.DOAnchorPos(originalPositions[idx], animDuration);
        }

        currentFocused = selectedButton;
        selectedButton.DOAnchorPos(centerPosition, animDuration);
        selectedButton.DOScale(focusScale, animDuration);
    }

    
    public void OnClickButton(int index)
    {
        FocusButton(buttons[index]);
    }
}

