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
        gm.speed = 25f;
        Vector3 pos = new Vector3(0f, 0f, 0f);
        Instantiate(player1, pos, Quaternion.identity);
        gm.ChangeState(GameManager.GameState.SURVIVAL);
    }

    public void PlaySingle()
    {
        gm.ChangeState(GameManager.GameState.DIFFICULTY_SINGLE);
    }
}
