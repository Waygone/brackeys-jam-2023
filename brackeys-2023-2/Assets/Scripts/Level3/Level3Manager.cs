using UnityEngine;

public class Level3Manager : MonoBehaviour
{
    public enum Level3State
    {
        NONE = 0,
        FOUND_MUSIC_SHEETS,
        FOUND_DECIPHER_BOOK,
        FOUND_PIANO_KEYS,
        PLAYED_FINAL_SONG_FORWARD,
        PLAYED_FINAL_SONG_BACKWARD,
    }

    public delegate void Level3StateChangeHandler(Level3State state);
    public event Level3StateChangeHandler OnLevel3StateChange;

    [SerializeField]
    private Level3Test _Level3Test;
    [SerializeField]
    private PianoPuzzle _PianoPuzzle;
    [SerializeField]
    private DialogueDB _DialogueDB;
    [SerializeField]
    private DialogueManager _DialogueManager;

    private readonly bool[] _hasFoundMusicSheets = new bool[3];
    private bool _hasFoundDecipherBook;
    private readonly bool[] _hasFoundPianoKeys = new bool[2];

    private Level3State _state;

    private void Start()
    {
        _PianoPuzzle.OnPianoPuzzleSolved += PianoPuzzleSolvedHandler;
        _PianoPuzzle.OnPianoPuzzleWrong += PianoPuzzleWrongHandler;

        _DialogueManager.OnDialogueEnd += (Dialogue dialogue) => { if (dialogue.Id == "piano-success") { LevelManager.Instance.TryJumpToLevel(1); } };

        Dialogue dialogue = _DialogueDB.GetDialogue("find-music-sheets");
        _DialogueManager.TrySetDialogue(dialogue);
        _DialogueManager.PlayDialogue();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            _DialogueManager.SkipPassage();
        }
    }

    private void Reset()
    {
        for (int i = 0; i < _hasFoundMusicSheets.Length; ++i)
        {
            _hasFoundMusicSheets[i] = false;
        }
        _hasFoundDecipherBook = false;
        for (int i = 0; i < _hasFoundPianoKeys.Length; ++i)
        {
            _hasFoundPianoKeys[i] = false;
        }

        _state = Level3State.NONE;
        OnLevel3StateChange?.Invoke(_state);
    }

    public void SetFoundMusicSheet(int index)
    {
        _hasFoundMusicSheets[index] = true;
        UpdateState();
    }
    public void SetFoundDecipherBook()
    {
        _hasFoundDecipherBook = true;
        UpdateState();
    }
    public void SetFoundPianoKeys(int index)
    {
        _hasFoundPianoKeys[index] = true;
        UpdateState();
    }

    private void UpdateState()
    {
        switch (_state)
        {
            case Level3State.NONE:
                {
                    for (int i = 0; i < _hasFoundMusicSheets.Length; ++i)
                    {
                        if (_hasFoundMusicSheets[i] == false)
                        {
                            return;
                        }
                    }

                    _state = Level3State.FOUND_MUSIC_SHEETS;
                    Dialogue dialogue = _DialogueDB.GetDialogue("find-decipher-book");
                    _DialogueManager.TrySetDialogue(dialogue);
                    _DialogueManager.PlayDialogue();
                    break;
                }
            case Level3State.FOUND_MUSIC_SHEETS:
                {
                    if (_hasFoundDecipherBook == false)
                    {
                        return;
                    }
                    _state = Level3State.FOUND_DECIPHER_BOOK;
                    Dialogue dialogue = _DialogueDB.GetDialogue("find-piano-keys");
                    _DialogueManager.TrySetDialogue(dialogue);
                    _DialogueManager.PlayDialogue();
                    break;
                }
            case Level3State.FOUND_DECIPHER_BOOK:
                {
                    for (int i = 0; i < _hasFoundPianoKeys.Length; ++i)
                    {
                        if (_hasFoundPianoKeys[i] == false)
                        {
                            return;
                        }
                    }
                    _state = Level3State.FOUND_PIANO_KEYS;
                    Dialogue dialogue = _DialogueDB.GetDialogue("play-the-piano");
                    _DialogueManager.TrySetDialogue(dialogue);
                    _DialogueManager.PlayDialogue();
                    break;
                }
        }

        OnLevel3StateChange?.Invoke(_state);
        Debug.LogWarning("Level3 " + _state);
    }

    private void PianoPuzzleSolvedHandler(int phase)
    {
        if (_state == Level3State.FOUND_PIANO_KEYS && phase == 0)
        {
            _state = Level3State.PLAYED_FINAL_SONG_FORWARD;
            Dialogue dialogue = _DialogueDB.GetDialogue("piano-hint-backwards");
            _DialogueManager.TrySetDialogue(dialogue);
            _DialogueManager.PlayDialogue();

            OnLevel3StateChange?.Invoke(_state);
        }

        if (_state == Level3State.PLAYED_FINAL_SONG_FORWARD && phase == 1)
        {
            _state = Level3State.PLAYED_FINAL_SONG_BACKWARD;
            Dialogue dialogue = _DialogueDB.GetDialogue("piano-success");
            _DialogueManager.TrySetDialogue(dialogue);
            _DialogueManager.PlayDialogue();
        }
    }
    private void PianoPuzzleWrongHandler(int phase)
    {
        if (phase == 0)
        {
            Dialogue dialogue = _DialogueDB.GetDialogue("piano-wrong-song");
            _DialogueManager.TrySetDialogue(dialogue);
            _DialogueManager.PlayDialogue();
        }
        else
        {
            Dialogue dialogue = _DialogueDB.GetDialogue("piano-hint-backwards-short");
            _DialogueManager.TrySetDialogue(dialogue);
            _DialogueManager.PlayDialogue();
        }
    }
}