using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager
{
    private static GameManager _instance;

    public enum GameState { MENU, SURVIVAL, DIFFICULTY_SINGLE, SINGLE, END_SURVIVAL, END_SINGLE, DIFFICULTY_SURVIVAL, INSTRUCTIONS };

    public GameState gameState { get; private set; }
    public int score;
    public float speed;
    public int difficulty;
    public int destroyedNPCs;
    public bool singleWin;
    public bool addHighscore;
    public delegate void ChangeStateDelegate();
    public static ChangeStateDelegate changeStateDelegate;

    public void ChangeState(GameState nextState)
    {
        gameState = nextState;
        changeStateDelegate();
    }

    public static GameManager GetInstance()
    {
        if (_instance == null)
        {
            _instance = new GameManager();
        }

        return _instance;
    }
    private GameManager()
    {
        //SceneManager.LoadScene(0);
        score = 0;
        destroyedNPCs = 0;
        singleWin = false;
        gameState = GameState.MENU;
    }

}
