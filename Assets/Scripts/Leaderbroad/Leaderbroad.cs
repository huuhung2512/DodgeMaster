using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Dan.Main;
public class Leaderbroad : MonoBehaviour
{
    [SerializeField]
    private List<TextMeshProUGUI> names;
    [SerializeField]
    private List<TextMeshProUGUI> scores;

    private string publicKeyLeaderbroad = "4a2c3379a102d6cfbfe743b01abeb3601e9c523653c439689ee586c442766746";
    private void Start()
    {
        GetLeaderBroad();
    }
    public void GetLeaderBroad()
    {
        LeaderboardCreator.GetLeaderboard(publicKeyLeaderbroad, ((msg) =>
        {
            int loopLength = (msg.Length < names.Count) ? msg.Length : names.Count;
            for (int i = 0; i < loopLength; i++)
            {
                names[i].text = msg[i].Username;
                scores[i].text = msg[i].Score.ToString();
            }
        }));
    }

    public void SetLeaderBroadEntry(string username, int score)
    {
        LeaderboardCreator.UploadNewEntry(publicKeyLeaderbroad, username, score, ((msg) =>
        {
            GetLeaderBroad();
        }));
    }
   
}
