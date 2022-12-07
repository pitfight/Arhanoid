using UnityEngine;
using UnityEngine.UI;

public class MenuManager : BaseManager
{
    [SerializeField] private Button continueGame;
    [SerializeField] private GameObject optionWindow;

    [SerializeField] private Slider music;
    [SerializeField] private Slider sfx;

    [SerializeField] private Toggle mainSound;

    private const string MUSIC_VOLUME = "music";
    private const string SFX_VOLUME = "sfx";
    private const string MAIN_SOUND = "sound";

    [Header("SFX sound")]
    [SerializeField] private AudioClip button;

    private void Awake()
    {
        GameState.Instance.LoadGame();
        continueGame.interactable = GameState.Instance.curentLevel != null ? true : false;
    }

    private void Start()
    {
        music.value = PlayerPrefs.GetFloat(MUSIC_VOLUME, 0.5f);
        sfx.value = PlayerPrefs.GetFloat(SFX_VOLUME, 0.5f);
        mainSound.isOn = DataSerialize.Instance.GetBool(SFX_VOLUME);
    }

    public void Continue()
    {
        LoadScene("Game");
    }

    public void NewGame()
    {
        GameState.Instance.NewGame();
        LoadScene("Game");
    }

    public void ShowOption()
    {
        optionWindow.SetActive(true);
    }

    public void SFX(float value)
    {
        SoundManager.Instance.ChangeVolumeSFX(value);
        PlayerPrefs.SetFloat(SFX_VOLUME, value);
        PlayerPrefs.Save();
    }

    public void Music(float value)
    {
        SoundManager.Instance.ChangeVolumeMusic(value);
        PlayerPrefs.SetFloat(MUSIC_VOLUME, value);
        PlayerPrefs.Save();
    }

    public void MainSound(bool value)
    {
        SoundManager.Instance.ToggleMute(value);
        DataSerialize.Instance.SetBool(MAIN_SOUND, value);
        PlayerPrefs.Save();
    }

    public void Button()
    {
        SoundManager.Instance.PlayOnSFX(button);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
