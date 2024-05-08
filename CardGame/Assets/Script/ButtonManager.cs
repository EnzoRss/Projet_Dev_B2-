using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;



public  class ButtonManager : MonoBehaviour
{
    public Player player ;
    public ToggleGroup toggleGroup;
    private Toggle activeToggle;
    public NetworkManager NetworkManager;

    public GameObject connect;
    public GameObject nameInput;
    public GameObject nameSelect;
    public GameObject toggleDeck1;
    public GameObject toggleDeck2;
    public GameObject toggleDeck3;
    public GameObject confirmDeck;

    public void NameSelection()
    {
        if (NetworkManager.IsConnected)
        {
            connect.SetActive(false);
            nameInput.SetActive(true);
            nameSelect.SetActive(true);   
        }
    }


    public  void CreatePlayer()
    {
        
        if (nameInput.GetComponent<TMP_InputField>() != null )
        {
            string surname = nameInput.GetComponent<TMP_InputField>().text;
            player = new Player(surname);
            nameSelect.SetActive(false);
            nameInput.SetActive(false);
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
        Deck deck = new Deck();
        Debug.Log(activeToggle.name);
        toggleDeck1.SetActive(false);
        toggleDeck2.SetActive(false);
        toggleDeck3.SetActive(false);
        confirmDeck.SetActive(false);
        player.deck = deck;
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
