using UnityEngine;

public class InteractionObjectTest : MonoBehaviour, IInteractable
{
    public string EnterInteract()
    {
        Debug.Log(this + " enter focus");
        return "Interact [E]";
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