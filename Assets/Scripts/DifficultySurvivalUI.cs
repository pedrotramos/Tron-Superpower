using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultySurvivalUI : MonoBehaviour
{
    public GameObject player1;
    public GameObject Obstaculo, ObstaculoG;
    GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.GetInstance();
    }

    void Play()
    {
        if (gm.difficulty == 3)
        {
            Instantiate(Obstaculo, new Vector3(35f, 35f, 0f), Quaternion.identity);
            Instantiate(Obstaculo, new Vector3(-35f, 35f, 0f), Quaternion.identity);
            Instantiate(Obstaculo, new Vector3(35f, -35f, 0f), Quaternion.identity);
            Instantiate(Obstaculo, new Vector3(-35f, -35f, 0f), Quaternion.identity);
        }
        Vector3 pos = new Vector3(0f, 0f, 0f);
        Instantiate(player1, pos, Quaternion.identity);
        gm.ChangeState(GameManager.GameState.SURVIVAL);
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
