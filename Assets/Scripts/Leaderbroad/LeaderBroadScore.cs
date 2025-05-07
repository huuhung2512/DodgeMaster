using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class LeaderBroadScore : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI inputScore;
    [SerializeField]
    private TMP_InputField inputName;

    public UnityEvent<string, int> submitScoreEvent;

    private void Awake()
    {
        inputScore.text = " "+PlayerPrefs.GetInt("BestScore", 0);
    }
    public void SubmitScore()
    {
        submitScoreEvent.Invoke(inputName.text, int.Parse(inputScore.text));
    }
    public void OnReturnClick()
    {
        UIManager.Instance.OnHideAllPanel();
        UIManager.Instance.OnShowPanelGameplay(true);
        AudioManager.Instance.PlaySound(GameEnum.ESound.ButtonClick);
    }
}
