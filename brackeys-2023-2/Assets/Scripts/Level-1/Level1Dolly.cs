using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Playables;

public class Level1Dolly : MonoBehaviour
{
    private PlayableDirector dollyDirector;

    private void Awake()
    {
        dollyDirector = GetComponent<PlayableDirector>();
    }

    public void PlayDolly()
    {
        dollyDirector.Play();
    }
}
