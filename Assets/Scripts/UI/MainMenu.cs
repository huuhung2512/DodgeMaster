using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;
using System;
public class MainMenu : SingletonBehavior<MainMenu>
{
    public TextMeshProUGUI coinsText;
    public void PlayGame()
    {
        SceneManager.LoadScene("Level 1");
    }
    public void OpenURL(string url){
        Application.OpenURL(url);
    }
    void Start()
    {
        int totalCoins = PlayerPrefs.GetInt("Coins", 0);
        coinsText.text = totalCoins.ToString();
    }

}
