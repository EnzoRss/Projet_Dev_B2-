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
    string ip; // Adresse IP du serveur
    string Path = "/API"; // Chemin de l'API
    string url; // URL compl�te de la requ�te
    public PlayerManager PlayerManager; // R�f�rence au gestionnaire de joueur
    public GameObject prefabInstance; // Instance du pr�fab � instancier pour repr�senter une carte sur l'interface utilisateur
    public GameObject InputIP; // InputField contenant l'adresse IP du serveur

    // M�thode pour obtenir l'adresse IP depuis l'InputField
    public void GetIp()
    {
        ip = InputIP.GetComponent<TMP_InputField>().text;
    }

    // Coroutine pour effectuer une requ�te GET
    IEnumerator GetRequest()
    {
        string requestUrl = $"http://{ip}{Path}{url}"; // Construire l'URL de la requ�te
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

    // Coroutine pour obtenir les informations du compte joueur
    IEnumerator GetAccount()
    {
        string requestUrl = $"http://{ip}{Path}{url}"; // Construire l'URL de la requ�te
        Debug.Log(requestUrl);
        using (UnityWebRequest request = UnityWebRequest.Get(requestUrl))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log(request.downloadHandler.text);
                // Convertir les donn�es JSON en objet PlayerData
                PlayerData data = JsonUtility.FromJson<PlayerData>(request.downloadHandler.text);
                // Mettre � jour les informations du joueur
                PlayerManager.player.username = data.username;
                PlayerManager.player.password = data.password;
                PlayerManager.player.id = (ushort)data.Id_users;
                PlayerManager.player.pv = 20;
                // D�marrer la r�cup�ration des cartes du joueur
                StartGetCards("/SelectDataJoin?table={\"table1\":\"cards\",\"table2\":\"decks_cards\"}&key={\"key1\":\"cards.Id_cards\",\"key2\":\"decks_cards.Id_Cards\"}&filter={\"decks_cards.Id_decks\":" + data.Id_decks + "}");
            }
            else
            {
                Debug.Log("Error: " + request.error);
            }
        }
    }

    // Coroutine pour obtenir les cartes du joueur
    IEnumerator GetCards()
    {
        string requestUrl = $"http://{ip}{Path}{url}"; // Construire l'URL de la requ�te
        Debug.Log(requestUrl);
        Deck deck = new Deck(1); // Cr�er un nouveau deck
        using (UnityWebRequest request = UnityWebRequest.Get(requestUrl))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                // Convertir le JSON re�u en tableau de Cartes
                DataCard[] cards = JsonConvert.DeserializeObject<DataCard[]>(request.downloadHandler.text);

                // V�rifier si deck.deck est null avant de l'utiliser
                if (deck.deck == null)
                {
                    Debug.Log("le deck et null");
                }

                // Ajouter chaque carte � la liste de cartes du deck
                foreach (DataCard card in cards)
                {
                    Card card1 = new Card();
                    card1.card_name = card.card_name;
                    card1.pv = int.Parse(card.pv);
                    card1.atk = int.Parse(card.atk);
                    card1.Id = int.Parse(card.Id_cards);
                    card1.description = card.description;
                    deck.deck.Add(card1);
                }

                // Mettre � jour le deck du joueur avec les cartes r�cup�r�es
                if (PlayerManager.player.deck == null)
                {
                    PlayerManager.player.deck = new Deck(1);
                }
                PlayerManager.player.deck = deck;
                PlayerManager.player.PrintPlayer();
                Debug.Log("Cartes r�cup�r�es avec succ�s !");
            }
            else
            {
                Debug.Log("Erreur lors de la r�cup�ration des cartes : " + request.error);
            }
        }
    }

    // Coroutine pour obtenir une carte sp�cifique
    IEnumerator GetCard()
    {
        string requestUrl = $"http://{ip}{Path}{url}"; // Construire l'URL de la requ�te
        Debug.Log(requestUrl);
        Deck deck = new Deck(1); // Cr�er un nouveau deck
        using (UnityWebRequest request = UnityWebRequest.Get(requestUrl))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                // Convertir le JSON re�u en objet DataCard
                DataCard card = JsonConvert.DeserializeObject<DataCard>(request.downloadHandler.text);

                // V�rifier si deck.deck est null avant de l'utiliser
                if (deck.deck == null)
                {
                    Debug.Log("le deck et null");
                }

                // Cr�er une nouvelle carte et la remplir avec les donn�es r�cup�r�es
                Card card1 = new Card();
                card1.card_name = card.card_name;
                card1.pv = int.Parse(card.pv);
                card1.atk = int.Parse(card.atk);
                card1.Id = int.Parse(card.Id_cards);
                card1.description = card.description;

                // Ajouter la carte � la liste de cartes sur le terrain de l'adversaire du joueur
                PlayerManager.player.deck.cardOnBoardEnemy.Add(card1);

                // Mettre � jour les informations des objets sur l'interface utilisateur avec les donn�es de la carte
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

                // Instancier l'objet repr�sentant la carte sur l'interface utilisateur
                GameObject temp = Instantiate(prefabInstance, new Vector3(-50 + ((1 * PlayerManager.player.deck.cardOnBoardEnemy.Count) * 150), 300), Quaternion.identity);
                PlayerManager.player.deck.cardOnBoardEnemyUI.Add(temp);
            }
            else
            {
                Debug.Log("Erreur lors de la r�cup�ration des cartes : " + request.error);
            }
        }
    }

    // M�thode pour d�marrer une requ�te GET g�n�rique
    public void StartGetRequest(string url)
    {
        this.url = url;
        StartCoroutine(GetRequest());
    }

    // M�thode pour d�marrer la r�cup�ration des informations du compte joueur
    public void StartGetPlayer(string url)
    {
        this.url = url;
        StartCoroutine(GetAccount());
    }

    // M�thode pour d�marrer la r�cup�ration des cartes du joueur
    public void StartGetCards(string url)
    {
        this.url = url;
        StartCoroutine(GetCards());
    }

    // M�thode pour d�marrer la r�cup�ration d'une carte sp�cifique
    public void StartGetCard(string url)
    {
        this.url = url;
        StartCoroutine(GetCard());
    }
}

// Classe repr�sentant les donn�es du joueur
[System.Serializable]
public class PlayerData
{
    public int Id_users; // Identifiant du joueur
    public string username; // Nom d'utilisateur du joueur
    public string password; // Mot de passe du joueur
    public int Id_decks; // Identifiant du deck du joueur
}

// Classe repr�sentant les donn�es d'une carte
public class DataCard
{
    public string Id_cards; // Identifiant de la carte
    public string card_name; // Nom de la carte
    public string description; // Description de la carte
    public string pv; // Points de vie de la carte
    public string atk; // Points d'attaque de la carte
    public string Id_decks; // Identifiant du deck contenant la carte
}
