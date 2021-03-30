using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManagerOn : MonoBehaviourPunCallbacks
{
    public static GameManagerOn Instancia { get; private set; }

    [SerializeField] private string localizacaoPrefab;
    [SerializeField] private Transform[] SpawnPoints;
    int numberPlayers = 0;

    private int jogadoresEmJogo = 0;

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
        photonView.RPC("AdicionaJogador",RpcTarget.AllBuffered);

        jogadores = new List<MovementPlayerOnline>();
    }

    [PunRPC]
    private void AdicionaJogador(){
        jogadoresEmJogo ++;
        if (jogadoresEmJogo == PhotonNetwork.PlayerList.Length){
            CriarJogador();
        }
    }

    private void CriarJogador(){
        var jogadorObj = PhotonNetwork.Instantiate("PlayerMulti", SpawnPoints[numberPlayers].position, Quaternion.identity);
        numberPlayers ++;

        var jogador = jogadorObj.GetComponent<MovementPlayerOnline>();

        jogador.photonView.RPC("Inicializa",RpcTarget.All, PhotonNetwork.LocalPlayer);
    }
}
