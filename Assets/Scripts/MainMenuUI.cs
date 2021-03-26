using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{

    GameManager gm;
    public GameObject player1, player2, player3, player4;
    public GameObject NPC1, NPC2, NPC3;

    private void OnEnable()
    {
        gm = GameManager.GetInstance();
    }

    public void PlaySurvival()
    {
        Vector3 pos = new Vector3(0f, 0f, 0f);
        Instantiate(player1, pos, Quaternion.identity);
        gm.ChangeState(GameManager.GameState.SURVIVAL);
    }

    public void PlaySingle()
    {
        Instantiate(player1, player1.transform.position, Quaternion.identity);
        Instantiate(NPC1, NPC1.transform.position, Quaternion.identity);
        Instantiate(NPC2, NPC2.transform.position, Quaternion.identity);
        Instantiate(NPC3, NPC3.transform.position, Quaternion.identity);
        gm.ChangeState(GameManager.GameState.SINGLE);
    }
}
