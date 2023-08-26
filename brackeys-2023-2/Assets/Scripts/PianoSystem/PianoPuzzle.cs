using UnityEngine;

public class PianoPuzzle : MonoBehaviour
{
    public delegate void PianoPuzzleSolvedHandler(int phase);
    public event PianoPuzzleSolvedHandler OnPianoPuzzleSolved;

    public delegate void PianoPuzzleWrongHandler(int phase);
    public event PianoPuzzleWrongHandler OnPianoPuzzleWrong;

    [SerializeField]
    private PianoManager _PianoManager;
    [SerializeField]
    private DialogueDB _DialogueDB;
    [SerializeField]
    private DialogueManager _DialogueManager;
    [SerializeField]
    private string[] _Sequence;


    private bool _isFirstPhase = true;
    private int _currentSequenceIndex;

    private void Start()
    {
        _PianoManager.OnPianoKeyPress += PianoKeyPressHandler;
    }

    private void PianoKeyPressHandler(PianoKey pianoKey)
    {
        // Already solved.
        if (_currentSequenceIndex >= _Sequence.Length)
        {
            return;
        }

        Debug.Log("Key press " + pianoKey.Key);

        int index = _currentSequenceIndex;
        if (_isFirstPhase)
        {
            index = _Sequence.Length - _currentSequenceIndex - 1;
        }

        if (pianoKey.Key == _Sequence[index])
        {
            Debug.Log("Key correct");
            ++_currentSequenceIndex;
            if (_currentSequenceIndex >= _Sequence.Length)
            {
                _currentSequenceIndex = 0;
                OnPianoPuzzleSolved?.Invoke(_isFirstPhase ? 0 : 1);
                _isFirstPhase = false;
            }
        }
        else
        {
            Debug.Log("Key wrong");
            _currentSequenceIndex = 0;
            OnPianoPuzzleWrong?.Invoke(_isFirstPhase ? 0 : 1);
        }
    }

    public void Reset()
    {
        _currentSequenceIndex = 0;
    }
}
