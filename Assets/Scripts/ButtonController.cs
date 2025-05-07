using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public void ReturnHome()
    {
        PlayerManager.Instance.QuitGame();
        AudioManager.Instance.PlaySound(GameEnum.ESound.ButtonClick);
    }
    public void ReloadGame()
    {
        PlayerManager.Instance.ReloadGame();
        AudioManager.Instance.PlaySound(GameEnum.ESound.ButtonClick);
    }
    public void TurnOnOffMusic()
    {
        AudioManager.Instance.ToggleMusic();
        AudioManager.Instance.PlaySound(GameEnum.ESound.ButtonClick);
    }
    public void TurnOnOffSound()
    {
        AudioManager.Instance.ToggleSound();
        AudioManager.Instance.PlaySound(GameEnum.ESound.ButtonClick);
    }
}
