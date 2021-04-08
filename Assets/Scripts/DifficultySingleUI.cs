using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultySingleUI : MonoBehaviour
{
    public GameObject player1, player2, player3, player4;
    public GameObject NPC1, NPC2, NPC3;
    GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.GetInstance();
    }

    void Play()
    {
        Instantiate(player1, player1.transform.position, Quaternion.identity);
        Instantiate(NPC1, NPC1.transform.position, Quaternion.identity);
        Instantiate(NPC2, NPC2.transform.position, Quaternion.identity);
        Instantiate(NPC3, NPC3.transform.position, Quaternion.identity);
        gm.ChangeState(GameManager.GameState.SINGLE);
    }

    public void PlayEasy()
    {
        gm.difficulty = 1;
        gm.speed = 25f;
        Play();
    }

    public void PlayNormal()
    {
        gm.difficulty = 2;
        gm.speed = 35f;
        Play();
    }
    public void PlayHard()
    {
        gm.difficulty = 3;
        gm.speed = 50f;
        Play();
    }

    public void BackToMainMenu()
    {
        gm.ChangeState(GameManager.GameState.MENU);
    }
}
