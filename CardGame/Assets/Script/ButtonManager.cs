using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;
using static UnityEngine.UIElements.UxmlAttributeDescription;



public  class ButtonManager : MonoBehaviour
{
    public PlayerManager PlayerManager;
    public ToggleGroup toggleGroup;
    private Toggle activeToggle;
    public NetworkManager NetworkManager;

    public GameObject connect;
    public GameObject nameInput;
    public GameObject PassInput;
    public GameObject nameSelect;
    public GameObject ConnectSelect;
    public GameObject toggleDeck1;
    public GameObject toggleDeck2;
    public GameObject toggleDeck3;
    public GameObject confirmDeck;
    public GameObject ButtonConnect;
    public GameObject ButtonCreate;
    public GameObject InputIP;
    public HttpManagement httpHandler;

    public void ConnectButton()
    {
       
        ButtonConnect.SetActive(false);
        ButtonCreate.SetActive(false); 
        nameInput.SetActive(true);
        PassInput.SetActive(true);
        ConnectSelect.SetActive(true);   
    }

    public void ConnectTry()
    {
        string username = nameInput.GetComponent<TMP_InputField>().text;
        string password = PassInput.GetComponent<TMP_InputField>().text;
        httpHandler.StartGetPlayer("/SelectData?table=users&filter={\"username\":\"" + username + "\",\"password\":\"" + password + "\"}"); ;
        Connect();
    }

    public void Connect()
    {
        nameInput.SetActive(false);
        PassInput.SetActive(false);
        ConnectSelect.SetActive(false);
        InputIP.SetActive(false);
        connect.SetActive(true);
        
    }

    public void CreateButton()
    {
        ButtonConnect.SetActive(false);
        ButtonCreate.SetActive(false);
        nameInput.SetActive(true);
        PassInput.SetActive(true);
        nameSelect.SetActive(true);
    }

    public  void CreatePlayer()
    {
        if (nameInput.GetComponent<TMP_InputField>() != null )
        {
            string username = nameInput.GetComponent<TMP_InputField>().text;
            string password = PassInput.GetComponent<TMP_InputField>().text;
            PlayerManager.player.username = username;
            PlayerManager.player.password = password;
            nameSelect.SetActive(false);
            nameInput.SetActive(false);
            PassInput.SetActive(false);
            InputIP.SetActive(false);
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

    public void SelectDeck()
    {
        Debug.Log(activeToggle.name);
        toggleDeck1.SetActive(false);
        toggleDeck2.SetActive(false);
        toggleDeck3.SetActive(false);
        confirmDeck.SetActive(false);
        httpHandler.StartGetCards("/SelectDataJoin?table={\"table1\":\"cards\",\"table2\":\"decks_cards\"}&key={\"key1\":\"cards.Id_cards\",\"key2\":\"decks_cards.Id_Cards\"}&filter={\"decks_cards.Id_decks\":"+ activeToggle.name +"}");
        httpHandler.StartGetRequest("/CreateData?table=users&data={\"username\":\"" + PlayerManager.player.username + "\",\"password\":\"" + PlayerManager.player.password + "\",\"Id_decks\":" + activeToggle.name + "}");
        Connect();
    }

    void Start()
    {
        // S'assurer qu'aucun bouton n'est sélectionné au début
        activeToggle = null;
    }

    public void OnToggleValueChanged(Toggle toggle)
    {
        if (toggle.isOn)
        {
            // Si un nouveau bouton est sélectionné, désélectionner l'ancien (s'il existe)
            if (activeToggle != null && activeToggle != toggle)
            {
                activeToggle.isOn = false;
            }

            activeToggle = toggle;
           
        }
        else if (activeToggle == toggle)
        {
            // Si le bouton désélectionné était le bouton actif, le marquer comme null
            activeToggle = null;
        }
    }
}
