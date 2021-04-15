using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultySingleUI : MonoBehaviour
{
    public GameObject player1, player2, player3, player4;
    public GameObject NPC1, NPC2, NPC3, Obstaculo, ObstaculoG;
    GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.GetInstance();
    }

    void Play()
    {
        if (gm.difficulty == 2)
        {
            Instantiate(ObstaculoG, ObstaculoG.transform.position, Quaternion.identity);
        }
        else if (gm.difficulty == 3)
        {
            Instantiate(Obstaculo, new Vector3(35f, 35f, 0f), Quaternion.identity);
            Instantiate(Obstaculo, new Vector3(-35f, 35f, 0f), Quaternion.identity);
            Instantiate(Obstaculo, new Vector3(35f, -35f, 0f), Quaternion.identity);
            Instantiate(Obstaculo, new Vector3(-35f, -35f, 0f), Quaternion.identity);
            Instantiate(Obstaculo, Obstaculo.transform.position, Quaternion.identity);
        }
        Instantiate(player1, player1.transform.position, Quaternion.identity);
        Instantiate(NPC1, NPC1.transform.position, Quaternion.identity);
        Instantiate(NPC2, NPC2.transform.position, Quaternion.identity);
        Instantiate(NPC3, NPC3.transform.position, Quaternion.identity);
        gm.ChangeState(GameManager.GameState.SINGLE);
        gm.addHighscore = true;
    }

    public void PlayEasy()
    {
        gm.difficulty = 1;
        gm.speed = 20f;
        Play();
    }

    public void PlayNormal()
    {
        gm.difficulty = 2;
        gm.speed = 30f;
        Play();
    }
    public void PlayHard()
    {
        //aleatorio = Random.Range(1,3);
        gm.difficulty = 3;
        gm.speed = 40f;
        Play();
    }

    public void BackToMainMenu()
    {
        gm.ChangeState(GameManager.GameState.MENU);
    }
}
