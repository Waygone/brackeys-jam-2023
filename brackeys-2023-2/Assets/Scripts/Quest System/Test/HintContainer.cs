using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintContainer : MonoBehaviour
{
    [SerializeField] private GameObject hintText;
    [SerializeField] private GameObject hintPressText;

    private bool hintsEnabled = false;

    public void ToggleHints()
    {
        hintsEnabled = !hintsEnabled;

        hintText.SetActive(hintsEnabled);
        hintPressText.SetActive(!hintsEnabled);
    }
}
