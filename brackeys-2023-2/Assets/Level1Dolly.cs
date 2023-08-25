using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Playables;

public class Level1Dolly : MonoBehaviour
{
    [SerializeField] private CinemachineSmoothPath dollyPath;

    private CinemachineVirtualCamera dollyCam;
    private PlayableDirector dollyDirector;

    private void Awake()
    {
        dollyCam = GetComponent<CinemachineVirtualCamera>();
        dollyDirector = GetComponent<PlayableDirector>();
    }

    private void Start()
    {
        dollyDirector.Play();
    }
}
