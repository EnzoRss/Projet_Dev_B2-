using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using static UnityEditor.Progress;


public class HttpManagement : MonoBehaviour
{
    public string ip;
    string url;
    public Player player;
    
    IEnumerator GetRequest()
    {
        string requestUrl = $"http://{ip}{url}";
        Debug.Log(requestUrl);
        using (UnityWebRequest request = UnityWebRequest.Get(requestUrl))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Received: " + request.downloadHandler.text);
            }
            else
            {
                Debug.Log("Error: " + request.error);
            }
        } 

    }

    IEnumerator GetAccount()
    {
        string requestUrl = $"http://{ip}{url}";
        Debug.Log(requestUrl);
        using (UnityWebRequest request = UnityWebRequest.Get(requestUrl))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log(request.downloadHandler.text);
                PlayerData data =JsonUtility.FromJson<PlayerData>(request.downloadHandler.text);
                Debug.Log(data.username);
                player.username = data.username ;
                player.password =data.password  ;
                player.id = (ushort)data.Id_users  ;
                StartGetCard("/SelectDataJoin?table={\"table1\":\"cards\",\"table2\":\"decks_cards\"}&key={\"key1\":\"cards.Id_cards\",\"key2\":\"decks_cards.Id_Cards\"}&filter={\"decks_cards.Id_decks\":" + data.Id_Decks + "}");
                player.PrintPlayer();
            
            }
            else
            {
                Debug.Log("Error: " + request.error);
            }
        }

    }


    IEnumerator GetCard()
    {
        string requestUrl = $"http://{ip}{url}";
        Debug.Log(requestUrl);
        Deck deck = new Deck(1);
        using (UnityWebRequest request = UnityWebRequest.Get(requestUrl))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                // Convertir le JSON reçu en tableau de Cartes
                Card[] cards = JsonConvert.DeserializeObject<Card[]>(request.downloadHandler.text);

                // Vérifier si deck.deck est null avant de l'utiliser
                if (deck.deck == null)
                {
                    Debug.Log("le deck et null");
                }

                // Ajouter chaque carte à la liste de cartes du deck
                foreach (Card card in cards)
                {
                    deck.deck.Add(card);
                }

                player.deck = deck;

                Debug.Log("Cartes récupérées avec succès !");
            }
            else
            {
                Debug.Log("Erreur lors de la récupération des cartes : " + request.error);
            }
        }
    }
    public void StartGetRequest(string url)
        {
            this.url = url;
            StartCoroutine(GetRequest());
        }
    public void StartGetCard(string url)
    {
        this.url = url;
        StartCoroutine(GetCard());
    }
    public void StartGetPlayer(string url)
    {
        this.url = url;
        StartCoroutine(GetAccount());
    }
}
[System.Serializable]
public class PlayerData
{
    public int Id_users;
    public string username;
    public string password;
    public int Id_Decks;
}
