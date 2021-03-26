using UnityEngine;
using UnityEngine.UI;

public class EndSingleUI : MonoBehaviour
{
    public Text message;

    GameManager gm;
    private void OnEnable()
    {
        gm = GameManager.GetInstance();
        if (gm.singleWin)
        {
            message.text = "You have won and scored " + gm.score.ToString() + " points!";
        }
        else
        {
            message.text = "You have lost. Your score is " + gm.score.ToString() + ".";
        }
    }

    public void BackToMainMenu()
    {
        gm.score = 0;
        gm.destroyedNPCs = 0;
        gm.singleWin = false;
        gm.ChangeState(GameManager.GameState.MENU);
    }
}