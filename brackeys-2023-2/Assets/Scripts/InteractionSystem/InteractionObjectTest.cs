using UnityEngine;

public class InteractionObjectTest : MonoBehaviour, IInteractable
{
    public void EnterInteract()
    {
        Debug.Log(this + " enter focus");
    }
    public void ExitInteract()
    {
        Debug.Log(this + " exit focus");
    }
    public void ClickInteract()
    {
        Debug.Log(this + " click");
    }
}