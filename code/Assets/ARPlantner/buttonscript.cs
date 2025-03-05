using UnityEngine;
using System.Collections;

public class ToggleCanvasGroup : MonoBehaviour
{
    public CanvasGroup[] ProposalViewUI;  // Elements in Proposal View
    public CanvasGroup[] EditViewUI;      // Elements in Edit View
    public float fadeDuration = 1f;       // Duration for fade effect
    public GameObject toggleButton;       // Rotating toggle button

    private bool isProposalViewActive = false;

    // 🔹 UI Elements for Specific Toggles
    public CanvasGroup[] ShowAllProposalsElements;  // Scroll component (Proposal List)
    public GameObject ShowAllProposalsButton;       // The rotating button for Show All Proposals

    public CanvasGroup[] EditViewToggleElements;  // The 5 buttons that appear in Edit View
    public GameObject EditToggleButton;          // The button that toggles these elements

    private bool isShowAllProposalsVisible = false;  // Tracks scroll list visibility
    private bool isEditViewToggleVisible = false;    // Tracks Edit View toggle state

    void Start()
    {
    // Ensure the correct view is shown at startup
    SetUIState(true);

    // Ensure Show All Proposals section is hidden at startup
    HideCanvasGroup(ShowAllProposalsElements);
    isShowAllProposalsVisible = false;

    // Ensure Edit View toggle buttons are hidden at startup
    HideCanvasGroup(EditViewToggleElements);
    isEditViewToggleVisible = false;

    // Ensure the Show All Proposals button rotation is reset
    if (ShowAllProposalsButton != null)
    {
        ShowAllProposalsButton.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    // Ensure the Edit Toggle button rotation is reset
    if (EditToggleButton != null)
    {
        EditToggleButton.transform.rotation = Quaternion.Euler(0, 0, 0);
    }
    }

    public void ToggleUI()
    {
        isProposalViewActive = !isProposalViewActive;

        if (toggleButton != null)
        {
            toggleButton.transform.Rotate(0, 0, isProposalViewActive ? 45 : -45);
        }

        if (isProposalViewActive)
        {
            // Hide Edit View elements and show Proposal View elements
            foreach (CanvasGroup field in EditViewUI) { StartCoroutine(FadeOut(field)); }
            foreach (CanvasGroup field in ProposalViewUI) { StartCoroutine(FadeIn(field)); }

            // Ensure Show All Proposals elements & rotations are reset
            ResetShowAllProposals();
        }
        else
        {
            // Hide Proposal View elements and show Edit View elements
            foreach (CanvasGroup field in ProposalViewUI) { StartCoroutine(FadeOut(field)); }
            foreach (CanvasGroup field in EditViewUI) { StartCoroutine(FadeIn(field)); }

            // Reset Edit View toggles
            ResetEditViewToggle();
        }
    }

    // 🔹 Toggle Show All Proposals
    public void ToggleShowAllProposals()
    {
        isShowAllProposalsVisible = !isShowAllProposalsVisible;

        foreach (CanvasGroup element in ShowAllProposalsElements)
        {
            if (isShowAllProposalsVisible)
                StartCoroutine(FadeIn(element));
            else
                StartCoroutine(FadeOut(element));
        }

        if (ShowAllProposalsButton != null)
        {
            ShowAllProposalsButton.transform.Rotate(0, 0, isShowAllProposalsVisible ? -90f : 90f);
        }
    }

    // 🔹 Toggle 5 Buttons in Edit View
    public void ToggleEditViewButtons()
    {
        isEditViewToggleVisible = !isEditViewToggleVisible;

        foreach (CanvasGroup button in EditViewToggleElements)
        {
            if (isEditViewToggleVisible)
                StartCoroutine(FadeIn(button));
            else
                StartCoroutine(FadeOut(button));
        }

        if (EditToggleButton != null)
        {
            EditToggleButton.transform.Rotate(0, 0, isEditViewToggleVisible ? 90f : -90f);
        }
    }

    // 🔹 Reset Show All Proposals elements when switching to Edit View
    private void ResetShowAllProposals()
    {
        isShowAllProposalsVisible = false;
        HideCanvasGroup(ShowAllProposalsElements);

        if (ShowAllProposalsButton != null)
        {
            ShowAllProposalsButton.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    // 🔹 Reset Edit View toggles when switching to Proposal View
    private void ResetEditViewToggle()
    {
        isEditViewToggleVisible = false;
        HideCanvasGroup(EditViewToggleElements);

        if (EditToggleButton != null)
        {
            EditToggleButton.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private void SetUIState(bool showProposalView)
    {
        isProposalViewActive = showProposalView;
        
        foreach (CanvasGroup field in ProposalViewUI)
        {
            field.alpha = showProposalView ? 1 : 0;
            field.interactable = showProposalView;
            field.blocksRaycasts = showProposalView;
        }

        foreach (CanvasGroup field in EditViewUI)
        {
            field.alpha = showProposalView ? 0 : 1;
            field.interactable = !showProposalView;
            field.blocksRaycasts = !showProposalView;
        }
    }

    private IEnumerator FadeIn(CanvasGroup field)
    {
        float timeElapsed = 0f;
        while (timeElapsed < fadeDuration)
        {
            field.alpha = Mathf.Lerp(0, 1, timeElapsed / fadeDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        field.alpha = 1;
        field.interactable = true;
        field.blocksRaycasts = true;
    }

    private IEnumerator FadeOut(CanvasGroup field)
    {
        float timeElapsed = 0f;
        while (timeElapsed < fadeDuration)
        {
            field.alpha = Mathf.Lerp(1, 0, timeElapsed / fadeDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        field.alpha = 0;
        field.interactable = false;
        field.blocksRaycasts = false;
    }

    private void HideCanvasGroup(CanvasGroup[] elements)
    {
        foreach (CanvasGroup element in elements)
        {
            element.alpha = 0;
            element.interactable = false;
            element.blocksRaycasts = false;
        }
    }
}