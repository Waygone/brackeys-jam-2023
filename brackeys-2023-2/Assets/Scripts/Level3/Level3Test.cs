using UnityEngine;

public class Level3Test : MonoBehaviour
{
    public delegate void PickupItemHandler(string item);
    public event PickupItemHandler OnPickupItem;

    [SerializeField]
    private Level3Manager _Level3Manager;

    [ContextMenu("CompleteFirst")]
    public void CompleteFirst()
    {
        OnPickupItem?.Invoke("MusicSheet0");
        OnPickupItem?.Invoke("MusicSheet1");
        OnPickupItem?.Invoke("MusicSheet2");
    }
    [ContextMenu("CompleteSecond")]
    public void CompleteSecond()
    {
        OnPickupItem?.Invoke("DecipherBook");
    }
    [ContextMenu("CompleteThird")]
    public void CompleteThird()
    {
        OnPickupItem?.Invoke("PianoKey0");
        OnPickupItem?.Invoke("PianoKey1");
    }
}