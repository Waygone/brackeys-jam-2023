using UnityEngine;

public class PianoTest : MonoBehaviour
{
    [SerializeField]
    private PianoPuzzle _PianoPuzzle;

    private void Start()
    {
        _PianoPuzzle.OnPianoPuzzleSolved += PianoPuzzleSolvedHandler;
        _PianoPuzzle.OnPianoPuzzleWrong += PianoPuzzleWrongHandler;
    }

    private void PianoPuzzleSolvedHandler()
    {
        Debug.LogWarning("Good job, you played correctly!");
    }
    private void PianoPuzzleWrongHandler()
    {
        Debug.LogWarning("Bad job, you played wrong!");
    }
}
