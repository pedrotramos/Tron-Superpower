using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManagerOn : MonoBehaviourPunCallbacks
{
    public static GameManagerOn Instancia { get; private set; }

    [SerializeField] private string localizacaoPrefab;
    [SerializeField] private Transform[] SpawnPoints;

    private int jogadoresEmJogo = 0;
    private int numberPlayers;

    public float speed;

    private List<MovementPlayerOnline> jogadores;

    public List<MovementPlayerOnline> Jogadores {get => jogadores; private set => jogadores = value;}


    private void Awake(){
        if (Instancia != null && Instancia != this){
            gameObject.SetActive(false);
            return;
        }
        Instancia = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start(){
        speed= 20f;
        numberPlayers = 1;

        photonView.RPC("AdicionaJogador",RpcTarget.AllBuffered);
        

        jogadores = new List<MovementPlayerOnline>();
    }

    [PunRPC]
    private void AdicionaJogador(){
        jogadoresEmJogo ++;
        if (jogadoresEmJogo == PhotonNetwork.PlayerList.Length && jogadoresEmJogo <= 4){
            CriarJogador();
        }
    }

    private void CriarJogador(){
        //string playername = string.Format("Player{0}", jogadoresEmJogo);
        // Debug.Log("JogadoresEmJogo");
        // Debug.Log(jogadoresEmJogo);
        var jogadorObj = PhotonNetwork.Instantiate("Player2", SpawnPoints[1].position, Quaternion.identity);
        var jogador = jogadorObj.GetComponent<MovementPlayerOnline>();
        jogador.photonView.RPC("Inicializa",RpcTarget.All, PhotonNetwork.LocalPlayer);
        Debug.Log("Numero:");
        Debug.Log(numberPlayers);
        numberPlayers++;
        // if (numberPlayers == 1)
        // {
        //     var jogadorObj = PhotonNetwork.Instantiate("Player1", SpawnPoints[0].position, Quaternion.identity);
        //     var jogador = jogadorObj.GetComponent<MovementPlayerOnline>();
        //     jogador.photonView.RPC("Inicializa",RpcTarget.All, PhotonNetwork.LocalPlayer);
        //     numberPlayers = 2;
        // }

        // else if (numberPlayers == 2)
        // {
        //     var jogadorObj = PhotonNetwork.Instantiate("Player2", SpawnPoints[1].position, Quaternion.identity);
        //     var jogador = jogadorObj.GetComponent<MovementPlayerOnline>();
        //     jogador.photonView.RPC("Inicializa",RpcTarget.All, PhotonNetwork.LocalPlayer);
        //     numberPlayers = 3;
        // }
        // else if (numberPlayers == 3)
        // {
        //     var jogadorObj = PhotonNetwork.Instantiate("Player3", SpawnPoints[2].position, Quaternion.identity);
        //     var jogador = jogadorObj.GetComponent<MovementPlayerOnline>();
        //     jogador.photonView.RPC("Inicializa",RpcTarget.All, PhotonNetwork.LocalPlayer);
        //     numberPlayers = 4;
        // }
        // else if (numberPlayers == 4)
        // {
        //     var jogadorObj = PhotonNetwork.Instantiate("Player4", SpawnPoints[3].position, Quaternion.identity);
        //     var jogador = jogadorObj.GetComponent<MovementPlayerOnline>();
        //     jogador.photonView.RPC("Inicializa",RpcTarget.All, PhotonNetwork.LocalPlayer);
        //     numberPlayers = 1;
        // }
        // Debug.Log("NumeroFinal:");
        // Debug.Log(numberPlayers);

    }
}
