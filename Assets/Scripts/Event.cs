using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Event : MonoBehaviour
{
    public void ReloadGame(){
        SceneManager.LoadScene("Level 1");
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySound(GameEnum.ESound.ButtonClick);
        }
    }
    public void QuitGame(){
        SceneManager.LoadScene("MainMenu");
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySound(GameEnum.ESound.ButtonClick);
        }
    }
}
