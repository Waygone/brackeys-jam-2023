using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyInteractable : MonoBehaviour, IInteractable
{
    [SerializeField]
    private Level2Manager _Level2Manager;
    [SerializeField]
    private int _KeyIndex;

    string IInteractable.EnterInteract()
    {
        return "Pickup [E]";
    }

    void IInteractable.ExitInteract()
    {

    }
    void IInteractable.ClickInteract()
    {
        _Level2Manager.SetFoundKey(_KeyIndex);
        Destroy(gameObject);
    }
}
