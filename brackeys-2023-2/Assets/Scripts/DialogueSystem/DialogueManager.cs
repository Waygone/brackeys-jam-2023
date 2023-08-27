using UnityEngine;
using System.Collections;
using System.Text;

public class DialogueManager : MonoBehaviour
{
    public delegate void DialogueBeginHandler(Dialogue dialogue);
    public event DialogueBeginHandler OnDialogueBegin;

    public delegate void PassageChangeHandler(DialoguePassage passage);
    public event PassageChangeHandler OnPassageChange;

    public delegate void DialogueMessageChangeHandler(string message);
    public event DialogueMessageChangeHandler OnDialogueMessageChange;

    public delegate void DialogueEndHandler(Dialogue dialogue);
    public event DialogueEndHandler OnDialogueEnd;

    // Dialogue begin played.
    private Dialogue _dialogue = null;
    private bool _skipPassage = false;
    private bool _isDialoguePlaying = false;

    public bool TrySetDialogue(Dialogue dialogue)
    {
        if (_isDialoguePlaying)
        {
            return false;
        }

        _dialogue = dialogue;
        return true;
    }

    public void SkipPassage()
    {
        _skipPassage = true;
    }

    // Plays the saved dialogue. To set a new dialogue, before playing
    // you have to call TrySetDialogue.
    public void PlayDialogue()
    {
        if (_isDialoguePlaying)
        {
            return;
        }

        Debug.Log("Playing dialogue2");

        StartCoroutine(PlayDialogueImpl());
    }
    private IEnumerator PlayDialogueImpl()
    {
        // Reset previous skip.
        _skipPassage = false;

        _isDialoguePlaying = true;
        Debug.Log("Playing dialogue3");
        OnDialogueBegin?.Invoke(_dialogue);

        for (int passageIndex = 0; passageIndex < _dialogue.DialoguePassages.Length; ++passageIndex)
        {
            DialoguePassage passage = _dialogue.DialoguePassages[passageIndex];
            OnPassageChange?.Invoke(passage);

            StringBuilder currentMessage = new(passage.Message.Length);
            int characterIndex = 0;
            while (characterIndex < passage.Message.Length)
            {
                // Early skip. Complete the whole passage.
                if (_skipPassage)
                {
                    OnDialogueMessageChange?.Invoke(passage.Message);
                    _skipPassage = false;
                    break;
                }

                AppendNextToken(passage, ref characterIndex, currentMessage);
                OnDialogueMessageChange?.Invoke(currentMessage.ToString());
                yield return new WaitForSeconds(passage.SecondsPerCharacter);
            }

            yield return new WaitUntil(() => _skipPassage);
            _skipPassage = false;
        }

        _isDialoguePlaying = false;
        OnDialogueEnd?.Invoke(_dialogue);
    }
    private void AppendNextToken(DialoguePassage passage, ref int index, StringBuilder builder)
    {
        // Escape Rich Text tags --> https://docs.unity3d.com/Packages/com.unity.textmeshpro@4.0/manual/RichText.html
        if (passage.Message[index] == '<')
        {
            int end = passage.Message.IndexOf('>', index);
            if (end == -1)
            {
                Debug.LogError($"Syntax error at {index}. Expected a closing '>' after it, but found none");
            }

            builder.Append(passage.Message.Substring(index, end - index + 1));
            index = end + 1;
        }
        else
        {
            builder.Append(passage.Message[index]);
            ++index;
        }
    }
}