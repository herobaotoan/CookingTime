using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Dan.Main;

public class LeaderBoard : MonoBehaviour
{
    [SerializeField]
    private List<TextMeshProUGUI> names;
    [SerializeField]
    private List<TextMeshProUGUI> scores;
    [SerializeField]
    private List<TextMeshProUGUI> coins;
    private string publicLeaderboardKey = "ab54996957af49decf01335d63a1c87930107c96ed4df14d66085c825e421ca7";

    private void OnEnable()
    {
        GetLeaderBoard();
    }
    public void GetLeaderBoard()
    {
        LeaderboardCreator.GetLeaderboard(publicLeaderboardKey, ((msg) =>
        {
            int loopLength = (msg.Length < names.Count) ? msg.Length : names.Count;
            for (int i = 0; i < loopLength; ++i)
            {
                names[i].text = msg[i].Username;
                scores[i].text = msg[i].Score.ToString();
                coins[i].text = msg[i].Extra;
            }
        }));
    }
    public void SetLeaderBoardEntry(string username, int score, string extra)
    {
        LeaderboardCreator.UploadNewEntry(publicLeaderboardKey, username, score, extra, ((msg) =>
        {
            GetLeaderBoard();
        }));
    }
}
