using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelGameplayUI : MonoBehaviour
{
    [SerializeField] private Button btnShop;
    [SerializeField] private Button btnSetting;
    [SerializeField] private Button btnPlay;
    [SerializeField] private Button btnLeaderBroad;

    private void OnEnable()
    {
        btnShop.onClick.AddListener(OnShopClick);
        btnSetting.onClick.AddListener(OnSettinglick);
        btnPlay.onClick.AddListener(OnPlayClick);
        btnLeaderBroad.onClick.AddListener(OnLeaderBroadClick);
    }
    private void OnDisable()
    {
        btnShop.onClick.RemoveListener(OnShopClick);
        btnSetting.onClick.RemoveListener(OnSettinglick);
        btnPlay.onClick.RemoveListener(OnPlayClick);
        btnLeaderBroad.onClick.RemoveListener(OnLeaderBroadClick);
    }
    private void OnShopClick()
    {
        UIManager.Instance.OnHideAllPanel();
        UIManager.Instance.OnShowPanelShop(true);
        AudioManager.Instance.PlaySound(GameEnum.ESound.ButtonClick);
    }
    private void OnSettinglick()
    {
        UIManager.Instance.OnHideAllPanel();
        UIManager.Instance.OnShowPanelSetting(true);
        AudioManager.Instance.PlaySound(GameEnum.ESound.ButtonClick);
    }
    private void OnPlayClick()
    {
        AudioManager.Instance.PlaySound(GameEnum.ESound.ButtonClick);
        MainMenu.Instance.PlayGame();
        UIManager.Instance.OnHideAllPanel();
    } 
    private void OnLeaderBroadClick()
    {
        UIManager.Instance.OnHideAllPanel();
        UIManager.Instance.OnShowPanelLeaderBroad(true);
        AudioManager.Instance.PlaySound(GameEnum.ESound.ButtonClick);
    }

}
