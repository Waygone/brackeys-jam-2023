using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PianoManager : MonoBehaviour, IInteractable
{
    public delegate void PianoKeyPressHandler(PianoKey pianoKey);
    public event PianoKeyPressHandler OnPianoKeyPress;

    [SerializeField]
    private PianoKeyScriptableObject[] _PianoKeys;
    [SerializeField]
    private PlayerController _PlayerController;
    [SerializeField]
    private Canvas _PianoCanvas;
    [SerializeField]
    private Image[] _KeyImages;
    [SerializeField]
    private Color _PressedKeyColor;

    private bool _isFocused = false;

    private void Update()
    {
        if (!_isFocused)
        {
            return;
        }

        for (int i = 0; i < _PianoKeys.Length; ++i)
        {
            if (Input.GetKeyDown(_PianoKeys[i].KeyboardKeyCode))
            {
                _KeyImages[i].color = _PressedKeyColor;
                OnPianoKeyPress(_PianoKeys[i].ToPianoKey());
            }

            if (Input.GetKeyUp(_PianoKeys[i].KeyboardKeyCode))
            {
                _KeyImages[i].color = Color.white;
                OnPianoKeyPress(_PianoKeys[i].ToPianoKey());
            }
        }
    }

    public string EnterInteract()
    {
        return "Play [E]";
    }

    public void ExitInteract()
    {

    }

    public void ClickInteract()
    {
        if (_isFocused)
        {
            _isFocused = false;
            _PianoCanvas.enabled = false;
            _PlayerController.EnableMovement();
        }
        else
        {
            _isFocused = true;
            _PianoCanvas.enabled = true;
            _PlayerController.DisableMovement();
        }
    }
}
