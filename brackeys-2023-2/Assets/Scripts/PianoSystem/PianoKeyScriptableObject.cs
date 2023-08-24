using UnityEngine;

[CreateAssetMenu(fileName = "PianoKey", menuName = "ScriptableObjects/Piano Key", order = 1)]
public class PianoKeyScriptableObject : ScriptableObject
{
    public string Key;
    public KeyCode KeyboardKeyCode;

    public PianoKey ToPianoKey()
    {
        return new PianoKey { Key = Key, KeyboardKeyCode = KeyboardKeyCode };
    }
}