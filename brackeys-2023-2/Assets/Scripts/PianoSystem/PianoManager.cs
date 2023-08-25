using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PianoManager : MonoBehaviour
{
    public delegate void PianoKeyPressHandler(PianoKey pianoKey);
    public event PianoKeyPressHandler OnPianoKeyPress;

    [SerializeField]
    private PianoKeyScriptableObject[] _PianoKeys;

    private void Update()
    {
        for (int i = 0; i < _PianoKeys.Length; ++i)
        {
            if (Input.GetKeyDown(_PianoKeys[i].KeyboardKeyCode))
            {
                OnPianoKeyPress(_PianoKeys[i].ToPianoKey());
            }
        }
    }
}
