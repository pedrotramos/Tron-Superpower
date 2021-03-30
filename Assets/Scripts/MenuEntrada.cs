using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuEntrada : MonoBehaviour
{
    [SerializeField] private Text nomeJogador;
    [SerializeField] private Text nomeSala;

    public void CriaSala(){
        GestorDeRede.Instancia.MudaNick(nomeJogador.text);
        GestorDeRede.Instancia.CriaSala(nomeSala.text);
    }

    public void EntraSala(){
        GestorDeRede.Instancia.MudaNick(nomeJogador.text);
        GestorDeRede.Instancia.EntraSala(nomeSala.text);
    }
}
