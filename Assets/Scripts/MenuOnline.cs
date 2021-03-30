using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class MenuOnline : MonoBehaviourPunCallbacks
{
    [SerializeField] private MenuEntrada menuEntrada;
    [SerializeField] private MenuLobby menuLobby;
    
    private void Start(){
        menuEntrada.gameObject.SetActive(false);
        menuLobby.gameObject.SetActive(false);

    }

    public override void OnConnectedToMaster(){
        Debug.Log("Conexão feita !");
    }

    public void MultiplayerTela(){
        menuEntrada.gameObject.SetActive(true);
    }

    public override void OnJoinedRoom(){
        MudaMenu(menuLobby.gameObject);
        menuLobby.photonView.RPC("AtualizaLista",RpcTarget.All);
    }

    public void MudaMenu(GameObject menu){
        menuEntrada.gameObject.SetActive(false);
        menuLobby.gameObject.SetActive(false);

        menu.SetActive(true);
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer){
        menuLobby.AtualizaLista();
    }

    public void SairLobby(){
        GestorDeRede.Instancia.SairDoLobby();
        MudaMenu(menuEntrada.gameObject);
    }

    
    public void ComecaJogo(string nomeCena){
        GestorDeRede.Instancia.photonView.RPC("ComecarJogo", RpcTarget.All, nomeCena);
    }






}
