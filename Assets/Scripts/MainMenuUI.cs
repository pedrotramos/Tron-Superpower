using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{

    GameManager gm;
    public GameObject player1;
    public GameObject player2;
    public GameObject NPC;

    private void OnEnable()
    {
        gm = GameManager.GetInstance();
    }

    public void PlaySurvival()
    {
        // SceneManager.LoadScene(0);
        Vector3 pos = new Vector3(0f, 0f, 0f);
        Instantiate(player1, pos, Quaternion.identity);
        gm.ChangeState(GameManager.GameState.SURVIVAL);
    }
}
