using System;
using UnityEngine;

public class AddTreeButton : MonoBehaviour
{
    public GameObject isSelectedPng;
    public int treeId;


    public void SelectThisTreeButtonClicked()
    {
        FindAnyObjectByType<TreeSelectionUiManager>().TreeWasSelected(treeId);
    }

    internal void UpdateSelectedStatus()
    {
        isSelectedPng.SetActive(TreeSelectionUiManager.selectedTree == treeId);
    }
}
