using UnityEngine;

public class Level2Manager : MonoBehaviour
{
    private readonly bool[] _hasFoundKeys = new bool[3];

    public void SetFoundKey(int index)
    {
        _hasFoundKeys[index] = true;
    }
}
