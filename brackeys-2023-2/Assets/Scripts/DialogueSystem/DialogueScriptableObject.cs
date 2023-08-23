using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "ScriptableObjects/Dialogue", order = 1)]
public class DialogueScriptableObject : ScriptableObject
{
    public string Id;
    public DialoguePassage[] DialoguePassages;

    public Dialogue ToDialogue()
    {
        return new Dialogue { Id = Id, DialoguePassages = DialoguePassages };
    }
}
