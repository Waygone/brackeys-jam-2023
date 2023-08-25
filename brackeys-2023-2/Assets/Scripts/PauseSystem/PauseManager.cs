using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public delegate void PlayHandler();
    public event PlayHandler OnPlay;

    public delegate void PauseHandler();
    public event PauseHandler OnPause;

    [SerializeField]
    private Canvas _PauseCanvas;
    [SerializeField]
    private MainMenuManager _MainMenuManager;

    private bool _isPlaying = true;

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isPlaying)
            {
                Pause();
            }
            else
            {
                Play();
            }
        }
    }

    public void Play()
    {
        _isPlaying = true;
        _PauseCanvas.enabled = false;

        OnPlay?.Invoke();
    }

    public void Pause()
    {
        _isPlaying = false;
        _MainMenuManager.Reset();
        _PauseCanvas.enabled = true;

        OnPause?.Invoke();
    }
}
