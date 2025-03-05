using System;
using UnityEngine;

public class TreeSelectionUiManager : MonoBehaviour
{
    public AddTreeButton[] treeButtons;
    public static int selectedTree = 2;

    public void TreeWasSelected(int treeId)
    {
        selectedTree = treeId;

        foreach (var tree in treeButtons)
        {
            tree.UpdateSelectedStatus();
        }
    }
}
