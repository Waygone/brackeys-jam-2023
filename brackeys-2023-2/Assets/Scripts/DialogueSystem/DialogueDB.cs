using UnityEngine;
using System.Collections.Generic;

public class DialogueDB : MonoBehaviour
{
    [SerializeField]
    private DialogueScriptableObject[] _Dialogues;

    private Dictionary<string, Dialogue> _dialoguesMap;

    private void Awake()
    {
        FillDialoguesMap();
    }

    // Fills the dialogues map to allow efficient lookups by dialogue's ID.
    private void FillDialoguesMap()
    {
        _dialoguesMap = new Dictionary<string, Dialogue>();
        for (int i = 0; i < _Dialogues.Length; ++i)
        {
            _dialoguesMap[_Dialogues[i].Id] = _Dialogues[i].ToDialogue();
        }
    }

    // Not recommended since it allocates memory at every call.
    public Dialogue GetDialogue(int index)
    {
        return _Dialogues[index].ToDialogue();
    }
    public Dialogue GetDialogue(string id)
    {
        return _dialoguesMap[id];
    }

    // Preferred way to retrieve dialogue safely.
    public bool TryGetDialogue(string id, out Dialogue result)
    {
        return _dialoguesMap.TryGetValue(id, out result);
    }
}