using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class ButtonManager : MonoBehaviour
{
    public PlayerManager PlayerManager; // Référence au gestionnaire de joueur
    public ToggleGroup toggleGroup; // Groupe de boutons de type Toggle
    private Toggle activeToggle; // Bouton de type Toggle actuellement sélectionné
    public NetworkManager NetworkManager; // Gestionnaire de réseau
    public GameObject connect; // GameObject représentant l'écran de connexion
    public GameObject nameInput; // GameObject contenant l'input du nom d'utilisateur
    public GameObject PassInput; // GameObject contenant l'input du mot de passe
    public GameObject nameSelect; // GameObject pour la sélection du nom
    public GameObject ConnectSelect; // GameObject pour la sélection de connexion
    public GameObject toggleDeck1; // GameObject du premier bouton de sélection de deck
    public GameObject toggleDeck2; // GameObject du deuxième bouton de sélection de deck
    public GameObject toggleDeck3; // GameObject du troisième bouton de sélection de deck
    public GameObject confirmDeck; // GameObject pour confirmer la sélection du deck
    public GameObject ButtonConnect; // Bouton de connexion
    public GameObject ButtonCreate; // Bouton de création de joueur
    public GameObject InputIP; // GameObject contenant l'adresse IP du serveur
    public HttpManagement httpHandler; // Gestionnaire des requêtes HTTP

    // Méthode pour afficher les éléments d'entrée pour la connexion
    public void ConnectButton()
    {
        ButtonConnect.SetActive(false);
        ButtonCreate.SetActive(false);
        nameInput.SetActive(true);
        PassInput.SetActive(true);
        ConnectSelect.SetActive(true);
    }

    // Méthode pour tenter la connexion
    public void ConnectTry()
    {
        string username = nameInput.GetComponent<TMP_InputField>().text;
        string password = PassInput.GetComponent<TMP_InputField>().text;
        // Démarrer la récupération des données du joueur depuis le serveur
        httpHandler.StartGetPlayer("/SelectData?table=users&filter={\"username\":\"" + username + "\",\"password\":\"" + password + "\"}"); ;
        Connect();
    }

    // Méthode pour terminer la connexion
    public void Connect()
    {
        nameInput.SetActive(false);
        PassInput.SetActive(false);
        ConnectSelect.SetActive(false);
        InputIP.SetActive(false);
        connect.SetActive(true);
    }

    // Méthode pour afficher les éléments d'entrée pour la création de joueur
    public void CreateButton()
    {
        ButtonConnect.SetActive(false);
        ButtonCreate.SetActive(false);
        nameInput.SetActive(true);
        PassInput.SetActive(true);
        nameSelect.SetActive(true);
    }

    // Méthode pour créer un joueur
    public void CreatePlayer()
    {
        if (nameInput.GetComponent<TMP_InputField>() != null)
        {
            string username = nameInput.GetComponent<TMP_InputField>().text;
            string password = PassInput.GetComponent<TMP_InputField>().text;
            // Mettre à jour les informations du joueur avec le nom d'utilisateur et le mot de passe
            PlayerManager.player.username = username;
            PlayerManager.player.password = password;
            nameSelect.SetActive(false);
            nameInput.SetActive(false);
            PassInput.SetActive(false);
            InputIP.SetActive(false);
            // Afficher les boutons de sélection de deck
            toggleDeck1.SetActive(true);
            toggleDeck2.SetActive(true);
            toggleDeck3.SetActive(true);
            confirmDeck.SetActive(true);
        }
        else
        {
            Debug.Log("aucun input field");
        }
    }

    // Méthode pour sélectionner un deck
    public void SelectDeck()
    {
        Debug.Log(activeToggle.name);
        toggleDeck1.SetActive(false);
        toggleDeck2.SetActive(false);
        toggleDeck3.SetActive(false);
        confirmDeck.SetActive(false);
        // Démarrer la récupération des cartes du deck sélectionné
        httpHandler.StartGetCards("/SelectDataJoin?table={\"table1\":\"cards\",\"table2\":\"decks_cards\"}&key={\"key1\":\"cards.Id_cards\",\"key2\":\"decks_cards.Id_Cards\"}&filter={\"decks_cards.Id_decks\":" + activeToggle.name + "}");
        // Envoyer les informations du joueur et du deck sélectionné au serveur
        httpHandler.StartGetRequest("/CreateData?table=users&data={\"username\":\"" + PlayerManager.player.username + "\",\"password\":\"" + PlayerManager.player.password + "\",\"Id_decks\":" + activeToggle.name + "}");
        Connect(); // Terminer la connexion
    }

    // Méthode appelée au démarrage du script
    void Start()
    {
        // S'assurer qu'aucun bouton n'est sélectionné au début
        activeToggle = null;
    }

    // Méthode appelée lorsqu'un bouton de type Toggle est sélectionné ou désélectionné
    public void OnToggleValueChanged(Toggle toggle)
    {
        if (toggle.isOn)
        {
            // Si un nouveau bouton est sélectionné, désélectionner l'ancien (s'il existe)
            if (activeToggle != null && activeToggle != toggle)
            {
                activeToggle.isOn = false;
            }
            activeToggle = toggle; // Définir le nouveau bouton sélectionné comme actif
        }
        else if (activeToggle == toggle)
        {
            // Si le bouton désélectionné était le bouton actif, le marquer comme null
            activeToggle = null;
        }
    }
}
