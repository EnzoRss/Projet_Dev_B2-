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
    public PlayerManager PlayerManager; // R�f�rence au gestionnaire de joueur
    public ToggleGroup toggleGroup; // Groupe de boutons de type Toggle
    private Toggle activeToggle; // Bouton de type Toggle actuellement s�lectionn�
    public NetworkManager NetworkManager; // Gestionnaire de r�seau
    public GameObject connect; // GameObject repr�sentant l'�cran de connexion
    public GameObject nameInput; // GameObject contenant l'input du nom d'utilisateur
    public GameObject PassInput; // GameObject contenant l'input du mot de passe
    public GameObject nameSelect; // GameObject pour la s�lection du nom
    public GameObject ConnectSelect; // GameObject pour la s�lection de connexion
    public GameObject toggleDeck1; // GameObject du premier bouton de s�lection de deck
    public GameObject toggleDeck2; // GameObject du deuxi�me bouton de s�lection de deck
    public GameObject toggleDeck3; // GameObject du troisi�me bouton de s�lection de deck
    public GameObject confirmDeck; // GameObject pour confirmer la s�lection du deck
    public GameObject ButtonConnect; // Bouton de connexion
    public GameObject ButtonCreate; // Bouton de cr�ation de joueur
    public GameObject InputIP; // GameObject contenant l'adresse IP du serveur
    public HttpManagement httpHandler; // Gestionnaire des requ�tes HTTP

    // M�thode pour afficher les �l�ments d'entr�e pour la connexion
    public void ConnectButton()
    {
        ButtonConnect.SetActive(false);
        ButtonCreate.SetActive(false);
        nameInput.SetActive(true);
        PassInput.SetActive(true);
        ConnectSelect.SetActive(true);
    }

    // M�thode pour tenter la connexion
    public void ConnectTry()
    {
        string username = nameInput.GetComponent<TMP_InputField>().text;
        string password = PassInput.GetComponent<TMP_InputField>().text;
        // D�marrer la r�cup�ration des donn�es du joueur depuis le serveur
        httpHandler.StartGetPlayer("/SelectData?table=users&filter={\"username\":\"" + username + "\",\"password\":\"" + password + "\"}"); ;
        Connect();
    }

    // M�thode pour terminer la connexion
    public void Connect()
    {
        nameInput.SetActive(false);
        PassInput.SetActive(false);
        ConnectSelect.SetActive(false);
        InputIP.SetActive(false);
        connect.SetActive(true);
    }

    // M�thode pour afficher les �l�ments d'entr�e pour la cr�ation de joueur
    public void CreateButton()
    {
        ButtonConnect.SetActive(false);
        ButtonCreate.SetActive(false);
        nameInput.SetActive(true);
        PassInput.SetActive(true);
        nameSelect.SetActive(true);
    }

    // M�thode pour cr�er un joueur
    public void CreatePlayer()
    {
        if (nameInput.GetComponent<TMP_InputField>() != null)
        {
            string username = nameInput.GetComponent<TMP_InputField>().text;
            string password = PassInput.GetComponent<TMP_InputField>().text;
            // Mettre � jour les informations du joueur avec le nom d'utilisateur et le mot de passe
            PlayerManager.player.username = username;
            PlayerManager.player.password = password;
            nameSelect.SetActive(false);
            nameInput.SetActive(false);
            PassInput.SetActive(false);
            InputIP.SetActive(false);
            // Afficher les boutons de s�lection de deck
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

    // M�thode pour s�lectionner un deck
    public void SelectDeck()
    {
        Debug.Log(activeToggle.name);
        toggleDeck1.SetActive(false);
        toggleDeck2.SetActive(false);
        toggleDeck3.SetActive(false);
        confirmDeck.SetActive(false);
        // D�marrer la r�cup�ration des cartes du deck s�lectionn�
        httpHandler.StartGetCards("/SelectDataJoin?table={\"table1\":\"cards\",\"table2\":\"decks_cards\"}&key={\"key1\":\"cards.Id_cards\",\"key2\":\"decks_cards.Id_Cards\"}&filter={\"decks_cards.Id_decks\":" + activeToggle.name + "}");
        // Envoyer les informations du joueur et du deck s�lectionn� au serveur
        httpHandler.StartGetRequest("/CreateData?table=users&data={\"username\":\"" + PlayerManager.player.username + "\",\"password\":\"" + PlayerManager.player.password + "\",\"Id_decks\":" + activeToggle.name + "}");
        Connect(); // Terminer la connexion
    }

    // M�thode appel�e au d�marrage du script
    void Start()
    {
        // S'assurer qu'aucun bouton n'est s�lectionn� au d�but
        activeToggle = null;
    }

    // M�thode appel�e lorsqu'un bouton de type Toggle est s�lectionn� ou d�s�lectionn�
    public void OnToggleValueChanged(Toggle toggle)
    {
        if (toggle.isOn)
        {
            // Si un nouveau bouton est s�lectionn�, d�s�lectionner l'ancien (s'il existe)
            if (activeToggle != null && activeToggle != toggle)
            {
                activeToggle.isOn = false;
            }
            activeToggle = toggle; // D�finir le nouveau bouton s�lectionn� comme actif
        }
        else if (activeToggle == toggle)
        {
            // Si le bouton d�s�lectionn� �tait le bouton actif, le marquer comme null
            activeToggle = null;
        }
    }
}
