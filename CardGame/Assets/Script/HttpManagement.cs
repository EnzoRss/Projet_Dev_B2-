using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using static UnityEditor.Progress;
using TMPro;
using UnityEngine.Windows;


public class HttpManagement : MonoBehaviour
{
    string ip;
    string Path = "/API/";
    string url;
    public PlayerManager PlayerManager;
    public GameObject prefabInstance;
    public GameObject InputIP;

    public void GetIp()
    {

        ip = InputIP.GetComponent<TMP_InputField>().text;
    }
    IEnumerator GetRequest()
    {
        string requestUrl = $"http://{ip}{Path}{url}";
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
        string requestUrl = $"http://{ip}{Path}{url}";
        Debug.Log(requestUrl);
        using (UnityWebRequest request = UnityWebRequest.Get(requestUrl))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log(request.downloadHandler.text);
                PlayerData data =JsonUtility.FromJson<PlayerData>(request.downloadHandler.text);
                PlayerManager.player.username = data.username ;
                PlayerManager.player.password =data.password  ;
                PlayerManager.player.id = (ushort)data.Id_users;
                PlayerManager.player.pv = 20;
                StartGetCards("/SelectDataJoin?table={\"table1\":\"cards\",\"table2\":\"decks_cards\"}&key={\"key1\":\"cards.Id_cards\",\"key2\":\"decks_cards.Id_Cards\"}&filter={\"decks_cards.Id_decks\":" + data.Id_decks + "}");
               // PlayerManager.player.PrintPlayer();
            
            }
            else
            {
                Debug.Log("Error: " + request.error);
            }
        }

    }


    IEnumerator GetCards()
    {
        string requestUrl = $"http://{ip}{Path}{url}";
        Debug.Log(requestUrl);
        Deck deck = new Deck(1);
        using (UnityWebRequest request = UnityWebRequest.Get(requestUrl))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                // Convertir le JSON reçu en tableau de Cartes
                DataCard[] cards = JsonConvert.DeserializeObject<DataCard[]>(request.downloadHandler.text);

                // Vérifier si deck.deck est null avant de l'utiliser
                if (deck.deck == null)
                {
                    Debug.Log("le deck et null");
                    
                } 
                // Ajouter chaque carte à la liste de cartes du deck
                foreach (DataCard card in cards)
                {
                    
                    Card card1 = new Card();
                    card1.card_name = card.card_name ;
                    card1.pv = int.Parse( card.pv);
                    card1.atk = int.Parse( card.atk);
                    card1.Id = int.Parse(card.Id_cards );
                    card1.description = card.description ;
                    deck.deck.Add(card1);
                }
                if (PlayerManager.player.deck == null)
                { PlayerManager.player.deck = new Deck(1); }
                PlayerManager.player.deck =  deck;
                PlayerManager.player.PrintPlayer();
                Debug.Log("Cartes récupérées avec succès !");
            }
            else
            {
                Debug.Log("Erreur lors de la récupération des cartes : " + request.error);
            }
        }
    }
    IEnumerator GetCard()
    {
        string requestUrl = $"http://{ip}{Path}{url}";
        Debug.Log(requestUrl);
        Deck deck = new Deck(1);
        using (UnityWebRequest request = UnityWebRequest.Get(requestUrl))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                // Convertir le JSON reçu en tableau de Cartes
                DataCard cards = JsonConvert.DeserializeObject<DataCard>(request.downloadHandler.text);

                // Vérifier si deck.deck est null avant de l'utiliser
                if (deck.deck == null)
                {
                    Debug.Log("le deck et null");

                }
                    
                  Card card1 = new Card();
                  card1.card_name = cards.card_name;
                  card1.pv = int.Parse(cards.pv);
                  card1.atk = int.Parse(cards.atk);
                  card1.Id = int.Parse(cards.Id_cards);
                  card1.description = cards.description;
                  PlayerManager.player.deck.cardOnBoardEnemy.Add(card1);
                  TextMeshProUGUI[] TextPrefab = prefabInstance.GetComponentsInChildren<TextMeshProUGUI>();

                foreach (TextMeshProUGUI textMeshProUGUI in TextPrefab)
                {
                    if (textMeshProUGUI.name == "PV")
                    {
                        textMeshProUGUI.text = "pv : " + card1.pv;
                    }
                    else if (textMeshProUGUI.name == "Name")
                    {
                        textMeshProUGUI.text = card1.card_name;
                    }
                    else if (textMeshProUGUI.name == "ATK")
                    {
                        textMeshProUGUI.text = "" + card1.atk;
                    }
                    else if (textMeshProUGUI.name == "Description")
                    {
                        textMeshProUGUI.text = card1.description;
                    }
                }
                Instantiate(prefabInstance, new Vector3(-107 + 225 * PlayerManager.player.deck.cardOnBoardEnemy.Count, 300), Quaternion.identity);
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

    public void StartGetCards(string url)
    {
       
        this.url = url;
        StartCoroutine(GetCards());
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
    public int Id_decks;
}
public class DataCard
{
    public string Id_cards;
    public string card_name;
    public string description;
    public string pv;
    public string atk;
    public string Id_decks;
}