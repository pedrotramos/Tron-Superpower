using UnityEngine;
using UnityEngine.UI;

public class EndSurvivalUI : MonoBehaviour
{
    public Text message;

    GameManager gm;
    private void OnEnable()
    {
        gm = GameManager.GetInstance();

        message.text = "You have scored " + gm.score.ToString() + " points!";
    }

    public void BackToMainMenu()
    {
        gm.score = 0;
        gm.ChangeState(GameManager.GameState.MENU);
    }
}
