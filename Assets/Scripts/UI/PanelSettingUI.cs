using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
public class PanelSettingUI : SingletonBehavior<PanelSettingUI>
{
    [SerializeField] private Button btnOnOffMusic;
    [SerializeField] private Button btnOnOffSound;
    [SerializeField] private Button btnReturn;
    [SerializeField] private Button btnShowTutorial;
    [SerializeField] private Button btnShowLinkFace;
    [SerializeField] private Button btnShowLinkInfo;
    [SerializeField] private Transform panelTutorial;
    [SerializeField] private Transform panelMainSetting;
    [SerializeField] private TextMeshProUGUI btnOnOffMusicTxt;
    [SerializeField] private TextMeshProUGUI btnOnOffSoundTxt;


    private void Start()
    {
        bool isMusicOn = PlayerPrefs.GetInt("MusicSetting", 1) == 1;
        bool isSoundOn = PlayerPrefs.GetInt("SoundSetting", 1) == 1;

        string musicText = isMusicOn ? "OFF" : "ON";
        string soundText = isSoundOn ? "OFF" : "ON";

        SetTextMusic(musicText);
        SetTextSound(soundText);
    }

    private void OnEnable()
    {
        btnOnOffMusic.onClick.AddListener(OnOnOffMusicClick);
        btnOnOffSound.onClick.AddListener(OnOnOffSoundClick);
        btnReturn.onClick.AddListener(OnReturnClick);
        btnShowTutorial.onClick.AddListener(OnShowTutorialClick);
        btnShowLinkFace.onClick.AddListener(OnShowLinkFace);
        btnShowLinkInfo.onClick.AddListener(OnShowLinkInfo);
    }


    private void OnDisable()
    {
        btnOnOffMusic.onClick.RemoveListener(OnOnOffMusicClick);
        btnOnOffSound.onClick.RemoveListener(OnOnOffSoundClick);
        btnReturn.onClick.RemoveListener(OnReturnClick);
        btnShowTutorial.onClick.RemoveListener(OnShowTutorialClick);
        btnShowLinkFace.onClick.RemoveListener(OnShowLinkFace);
        btnShowLinkInfo.onClick.RemoveListener(OnShowLinkInfo);
    }
    private void OnShowLinkInfo()
    {
        MainMenu.Instance.OpenURL("https://hoc21.itch.io/");
        AudioManager.Instance.PlaySound(GameEnum.ESound.ButtonClick);
    }

    private void OnShowLinkFace()
    {
        MainMenu.Instance.OpenURL("https://www.facebook.com/duchoc.bin2112");
        AudioManager.Instance.PlaySound(GameEnum.ESound.ButtonClick);
    }

    private void OnShowTutorialClick()
    {
        panelMainSetting.gameObject.SetActive(false);
        panelTutorial.gameObject.SetActive(true);
        AudioManager.Instance.PlaySound(GameEnum.ESound.ButtonClick);
    }

    public void OnReturnClick()
    {
        UIManager.Instance.OnHideAllPanel();
        UIManager.Instance.OnShowPanelGameplay(true);
        AudioManager.Instance.PlaySound(GameEnum.ESound.ButtonClick);
    }
    public void SetTextSound(string text)
    {
        btnOnOffSoundTxt.text = text;
        PlayerPrefs.SetString("SFXText", text);
        PlayerPrefs.Save();
    }

    public void SetTextMusic(string text)
    {
        btnOnOffMusicTxt.text = text;
        PlayerPrefs.SetString("MusicText", text);
        PlayerPrefs.Save();
    }

    private void OnOnOffSoundClick()
    {
        AudioManager.Instance.ToggleSound();
        bool isSoundOn = PlayerPrefs.GetInt("SoundSetting", 1) == 1;
        SetTextSound(isSoundOn ? "OFF" : "ON"); 
        AudioManager.Instance.PlaySound(GameEnum.ESound.ButtonClick);
    }

    private void OnOnOffMusicClick()
    {
        AudioManager.Instance.ToggleMusic();
        bool isMusicOn = PlayerPrefs.GetInt("MusicSetting", 1) == 1;
        SetTextMusic(isMusicOn ? "OFF" : "ON"); 
        AudioManager.Instance.PlaySound(GameEnum.ESound.ButtonClick);
    }

}
