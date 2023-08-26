using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private Canvas _MainCanvas;
    [SerializeField]
    private Canvas _SettingsCanvas;
    [SerializeField]
    private Canvas _CreditsCanvas;

    [SerializeField]
    private Button _MainPlayButton;
    [SerializeField]
    private Button _MainSettingsButton;
    [SerializeField]
    private Button _MainCreditsButton;
    [SerializeField]
    private Button _MainQuitButton;
    [SerializeField]
    private Slider _MainMainVolumeSlider;
    [SerializeField]
    private Slider _MainMusicVolumeSlider;

    [SerializeField]
    private Button _SettingsBackButton;
    [SerializeField]
    private Button _CreditsBackButton;

    [SerializeField]
    private PauseManager _PauseManager;

    private void Start()
    {
        _MainPlayButton.onClick.AddListener(MainCanvasClickPlayHandler);
        _MainSettingsButton.onClick.AddListener(MainSettingsClickCreditsHandler);
        _MainCreditsButton.onClick.AddListener(MainCanvasClickCreditsHandler);
        _MainQuitButton.onClick.AddListener(MainCanvasClickQuitHandler);
        _MainMainVolumeSlider.onValueChanged.AddListener(MainMainVolumeSliderChangeHandler);
        _MainMusicVolumeSlider.onValueChanged.AddListener(MainMusicVolumeSliderChangeHandler);

        _SettingsBackButton.onClick.AddListener(SettingsCanvasClickBackHandler);
        _CreditsBackButton.onClick.AddListener(CreditCanvasClickBackHandler);
    }

    private void MainCanvasClickPlayHandler()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            LevelManager.Instance.TryAdvanceToNextLevel();
        }
        else
        {
            _PauseManager.Play();
        }
    }
    private void MainSettingsClickCreditsHandler()
    {
        _MainCanvas.enabled = false;
        _SettingsCanvas.enabled = true;

        _MainMainVolumeSlider.value = GlobalData.MainVolume;
        _MainMusicVolumeSlider.value = GlobalData.MusicVolume;
    }
    private void MainCanvasClickCreditsHandler()
    {
        _MainCanvas.enabled = false;
        _CreditsCanvas.enabled = true;
    }
    private void MainCanvasClickQuitHandler()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
    private void MainMainVolumeSliderChangeHandler(float value)
    {
        GlobalData.MainVolume = (int)value;
        Debug.Log("Main volume " + GlobalData.MainVolume);
    }
    private void MainMusicVolumeSliderChangeHandler(float value)
    {
        GlobalData.MusicVolume = (int)value;
        Debug.Log("Music volume " + GlobalData.MusicVolume);
    }

    private void SettingsCanvasClickBackHandler()
    {
        _SettingsCanvas.enabled = false;
        _MainCanvas.enabled = true;
    }
    private void CreditCanvasClickBackHandler()
    {
        _CreditsCanvas.enabled = false;
        _MainCanvas.enabled = true;
    }

    public void Reset()
    {
        _MainCanvas.enabled = true;
        _SettingsCanvas.enabled = false;
        _CreditsCanvas.enabled = false;
    }
}
