using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{

    GameManager gm;
    public GameObject player1;
    public GameObject NPC1, NPC2, NPC3;

    private void OnEnable()
    {
        gm = GameManager.GetInstance();
    }

    public void PlaySurvival()
    {
        gm.ChangeState(GameManager.GameState.DIFFICULTY_SURVIVAL);
    }

    public void PlaySingle()
    {
        gm.ChangeState(GameManager.GameState.DIFFICULTY_SINGLE);
    }

    public void ShowInstructions()
    {
        gm.ChangeState(GameManager.GameState.INSTRUCTIONS);
    }

}
