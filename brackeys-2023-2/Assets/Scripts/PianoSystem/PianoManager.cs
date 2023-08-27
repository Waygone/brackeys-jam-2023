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
    private PianoPuzzle _PianoPuzzle;
    [SerializeField]
    private Canvas _PianoCanvas;
    [SerializeField]
    private Image[] _KeyImages;
    [SerializeField]
    private Color _PressedKeyColor;
    [SerializeField]
    private DialogueManager _DialogueManager;

    private bool _isFocused = false;
    private bool _isControlled = true;

    private void Start()
    {
        _DialogueManager.OnDialogueBegin += (Dialogue dialogue) => _isControlled = false;
        _DialogueManager.OnDialogueEnd += (Dialogue dialogue) => _isControlled = true;
    }

    private void Update()
    {
        if (!_isFocused)
        {
            return;
        }

        for (int i = 0; i < _PianoKeys.Length; ++i)
        {
            if (Input.GetKeyDown(_PianoKeys[i].KeyboardKeyCode) && _isControlled)
            {
                _KeyImages[i].color = _PressedKeyColor;
                OnPianoKeyPress(_PianoKeys[i].ToPianoKey());
            }

            if (Input.GetKeyUp(_PianoKeys[i].KeyboardKeyCode))
            {
                _KeyImages[i].color = Color.white;
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
            _PianoPuzzle.Reset();
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
