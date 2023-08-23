using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "MovePath", menuName = "ScriptableObjects/NpcMovePath")]
public class NpcMovePath : ScriptableObject
{
    [Header("Path Details")]
    [SerializeField] private MovementDirection[] pathDirections;

    [SerializeField] private bool loop;
    [SerializeField] private float timeWaitAtEachPosition = 0.1f;
    [SerializeField] private float timeWaitAtEachTurn = 0.5f;

    [SerializeField] private StartPosition startPosition;

    [Header("Optional Parameters")]
    [SerializeField] private string levelName;

    [SerializeField] private string questId;
    
    [SerializeField] private string objectiveId;

    public MovementDirection[] PathDirections
    {
        get => pathDirections;
        private set => pathDirections = value;
    }

    public bool Loop
    {
        get => loop;
        private set => loop = value;
    }

    public StartPosition StartPosition
    {
        get => startPosition; 
        private set => startPosition = value;
    }

    public float TimeWaitAtEachPosition
    {
        get => timeWaitAtEachPosition;
        set => timeWaitAtEachPosition = value;
    }

    public float TimeWaitAtEachTurn
    {
        get => timeWaitAtEachTurn;
        set => timeWaitAtEachTurn = value;
    }

    public bool ValidMovePath()
    {
        return CorrectLevel() && CorrectQuest() && CorrectObjective();
    }

    private bool CorrectLevel() => !string.IsNullOrEmpty(levelName) ? levelName == SceneManager.GetActiveScene().name : true;
    private bool CorrectQuest() => !string.IsNullOrEmpty(questId) ? QuestManager.instance.currentQuest?.QuestId == questId : true;
    private bool CorrectObjective() => !string.IsNullOrEmpty(objectiveId) ? QuestManager.instance.currentQuest?.CurrentObjective.ObjectiveID == objectiveId : true;
}

[System.Serializable]
public struct StartPosition
{
    public bool hasStartPosition;
    public Vector2Int startPosition;
}
