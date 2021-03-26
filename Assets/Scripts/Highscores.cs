using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Referências:
//      1. https://www.grimoirehex.com/unity-3d-local-leaderboard/

public class PlayerInfo
{
    public string name;
    public int score;

    public PlayerInfo(string name, int score)
    {
        this.name = name;
        this.score = score;
    }
}

public class Highscores : MonoBehaviour
{
    GameManager gm;
    public InputField username;
    public Text position;
    public Text scoreNumber;
    public Text playerName;
    List<PlayerInfo> collectedStats;

    void Start()
    {
        gm = GameManager.GetInstance();

        // Lista contendo as informações dos jogadores
        collectedStats = new List<PlayerInfo>();

        position.text = "";
        scoreNumber.text = "";
        playerName.text = "";
        loadHighscores();
    }

    public void SubmitButton()
    {
        // Um determinado highscore só enviado se o username não estiver vazio
        if (username.text.Length > 0 && username.enabled)
        {
            // Cria um objeto usando o InputField para o username e a pontuação do jogador
            // obtida através do GameManager
            PlayerInfo stats = new PlayerInfo(username.text, gm.score);

            // Adiciona as informações do novo jogador à lista
            collectedStats.Add(stats);

            //faz sort para deixar visivel somente os maiores scores
            sortStats();
        }
    }

    void sortStats()
    {
        // Começa no final da lista e compara o score com o registrado acima dele
        // se o score obtido for maior do que o registrado acima dele eles trocam de lugar
        // caso contrário o loop para
        for (int i = collectedStats.Count - 1; i > 0; i--)
        {
            if (collectedStats[i].score > collectedStats[i - 1].score)
            {
                PlayerInfo tempInfo = collectedStats[i - 1];
                collectedStats[i - 1] = collectedStats[i];
                collectedStats[i] = tempInfo;
            }
            else break;
        }

        // Atualiza o PlayerPref que guarda os valores dos highscores
        updatePlayerPrefsString();
    }

    void updatePlayerPrefsString()
    {
        // Começa com uma string vazia
        string stats = "";

        // Adiciona cada nome e score da lista
        for (int i = 0; i < collectedStats.Count; i++)
        {
            // As vírgulas são usadas para separar os nomes e scores
            stats += collectedStats[i].name + ",";
            stats += collectedStats[i].score + ",";
        }

        //salva a string
        if (gm.gameState == GameManager.GameState.END_SURVIVAL)
        {
            PlayerPrefs.SetString("Survival_Highscores", stats);
        }

        updateHighscores();
    }

    void updateHighscores()
    {
        // Inicializa cada caixa de texto vazia
        position.text = "";
        scoreNumber.text = "";
        playerName.text = "";

        // Percorre a lista de highscores coletados e constrói
        // a string final completa
        for (int i = 0; i < collectedStats.Count; i++)
        {
            position.text += (i + 1).ToString();
            if (i + 1 == 1) position.text += "ST\n";
            else if (i + 1 == 2) position.text += "ND\n";
            else if (i + 1 == 3) position.text += "RD\n";
            else position.text += "TH\n";

            scoreNumber.text += collectedStats[i].score.ToString() + "\n";

            playerName.text += collectedStats[i].name + "\n";
        }
    }

    void loadHighscores()
    {
        string stats = "";
        // Carrega a string de highscores que está salva em PlayerPrefs
        if (gm.gameState == GameManager.GameState.END_SURVIVAL)
        {
            stats = PlayerPrefs.GetString("Highscores", "");
        }

        // Cria uma lista a partir da string recebida
        // A estrutura da string possui todos os elementos separados por
        // vírgulas e nome e pontuação estão aos pares
        // Ou seja, "nome1, pontuação1, nome2, pontuação2, ..."
        string[] stats_split = stats.Split(',');

        for (int i = 0; i < stats_split.Length - 2; i += 2)
        {
            // Cria um objeto PlayerInfo a partir das informações obtidas
            PlayerInfo loadedInfo = new PlayerInfo(stats_split[i], int.Parse(stats_split[i + 1]));

            // Adiciona o objeto criado à lista de highscores coletados
            collectedStats.Add(loadedInfo);

            // Atualiza o display com as novas informações
            updateHighscores();
        }
    }

    public void ClearPlayerPrefs()
    {
        //Deleta os highscores que estavam salvos
        if (gm.gameState == GameManager.GameState.END_SURVIVAL)
        {
            PlayerPrefs.DeleteKey("Highscores");
        }

        //Remove os antigos highscores do display
        position.text = "";
        scoreNumber.text = "";
        playerName.text = "";

        //Garante que o display não irá exibir lixo
        Start();
    }
}