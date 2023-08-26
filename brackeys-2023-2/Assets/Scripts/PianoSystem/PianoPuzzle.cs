using UnityEngine;

public class PianoPuzzle : MonoBehaviour
{
    public delegate void PianoPuzzleSolvedHandler();
    public event PianoPuzzleSolvedHandler OnPianoPuzzleSolved;

    public delegate void PianoPuzzleWrongHandler();
    public event PianoPuzzleWrongHandler OnPianoPuzzleWrong;

    [SerializeField]
    private PianoManager _PianoManager;
    [SerializeField]
    private string[] _Sequence;


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

        if (pianoKey.Key == _Sequence[_currentSequenceIndex])
        {
            Debug.Log("Key correct");
            ++_currentSequenceIndex;
            if (_currentSequenceIndex >= _Sequence.Length)
            {
                _currentSequenceIndex = 0;
                OnPianoPuzzleSolved?.Invoke();
            }
        }
        else
        {
            Debug.Log("Key wrong");
            _currentSequenceIndex = 0;
            OnPianoPuzzleWrong?.Invoke();
        }
    }
}
