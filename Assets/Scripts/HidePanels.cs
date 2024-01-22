using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidePanels : MonoBehaviour
{
    [SerializeField] private GameObject[] panelsToHide;

    public void OnHidePanelToggle(bool isHidden) {
        foreach (GameObject panel in panelsToHide)
            panel.SetActive(isHidden);
    }
}
