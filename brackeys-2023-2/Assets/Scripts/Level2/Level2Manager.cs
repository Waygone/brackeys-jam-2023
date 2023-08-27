using UnityEngine;

public class Level2Manager : MonoBehaviour
{
    public delegate void Level2FoundAllKeysHandler();
    public event Level2FoundAllKeysHandler OnLevel2FoundAllKeys;

    private readonly bool[] _hasFoundKeys = new bool[3];

    public void SetFoundKey(int index)
    {
        _hasFoundKeys[index] = true;
        UpdateState();
    }

    private void UpdateState()
    {
        for (int i = 0; i < _hasFoundKeys.Length; ++i)
        {
            if (_hasFoundKeys[i] == false)
            {
                return;
            }
        }

        OnLevel2FoundAllKeys?.Invoke();
        Debug.LogWarning("Level2 completed!");
    }
}
