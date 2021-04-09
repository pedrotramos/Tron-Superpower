using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    private static GameManager _instance;

    public enum GameState { MENU, SURVIVAL, DIFFICULTY_SINGLE, SINGLE, END_SURVIVAL, END_SINGLE, DIFFICULTY_SURVIVAL };

    public GameState gameState { get; private set; }
    public int score;
    public float speed;
    public int difficulty;
    public int destroyedNPCs;
    public bool singleWin;
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
        score = 0;
        destroyedNPCs = 0;
        singleWin = false;
        gameState = GameState.MENU;
    }

}
