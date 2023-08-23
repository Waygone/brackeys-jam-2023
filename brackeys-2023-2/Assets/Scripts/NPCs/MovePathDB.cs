using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MovePathDB : MonoBehaviour
{
    [SerializeField] private NpcMovePath[] npcMovePaths = null;

    private NpcMovePath currentNpcMovePath = null;

    private NpcMovement npcMovement;

    private void Awake()
    {
        npcMovement = GetComponent<NpcMovement>();
    }

    private void Start()
    {
        //QuestManager.instance.QuestOrObjectiveUpdated.AddListener(UpdateMovePath);
        UpdateMovePath();
    }

    private void UpdateMovePath()
    {
        currentNpcMovePath = npcMovePaths.LastOrDefault(x => x.ValidMovePath());
    }
}
