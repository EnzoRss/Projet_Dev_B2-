using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Deck
{
    int id; // Identifiant du deck
    public string name; // Nom du deck
    public List<Card> deck; // Liste de cartes dans le deck
    public List<Card> cardInHand; // Liste de cartes en main
    public List<Card> cardOnBoard; // Liste de cartes sur le terrain
    public List<GameObject> cardInHandUI; // Liste d'objets repr�sentant les cartes en main (interface utilisateur)
    public List<GameObject> cardOnBoardUI; // Liste d'objets repr�sentant les cartes sur le terrain (interface utilisateur)

    public List<Card> cardOnBoardEnemy; // Liste de cartes sur le terrain de l'adversaire
    public List<GameObject> cardOnBoardEnemyUI; // Liste d'objets repr�sentant les cartes sur le terrain de l'adversaire (interface utilisateur)

    // M�thode pour piocher une carte depuis le deck
    public Card DrawCard()
    {
        if (deck.Count <= 0)
        {
            // Si le deck est vide, retourne une carte vide
            return new Card();
        }
        int randomNumber = Random.Range(1, deck.Count - 1); // G�n�re un nombre al�atoire pour choisir une carte
        Card cardTodeck = deck.ElementAt(randomNumber); // S�lectionne la carte � piocher
        cardInHand.Add(deck.ElementAt(randomNumber)); // Ajoute la carte � la main
        deck.RemoveAt(randomNumber); // Retire la carte du deck
        return cardTodeck; // Retourne la carte pioch�e
    }

    // Constructeur de la classe Deck
    public Deck(int id)
    {
        this.id = id;
        deck = new List<Card>();
        cardInHand = new List<Card>();
        cardOnBoard = new List<Card>();
        cardOnBoardUI = new List<GameObject>();
        cardInHandUI = new List<GameObject>();
        cardOnBoardEnemy = new List<Card>();
        cardOnBoardEnemyUI = new List<GameObject>();
    }

    // M�thode pour afficher les informations des cartes sur l'interface utilisateur
    public void PrintCardUI()
    {
        foreach (GameObject card in cardOnBoardUI)
        {
            card.GetComponentInChildren<TextMeshProUGUI>(); // R�cup�re le composant TextMeshProUGUI de l'objet carte
            TextMeshProUGUI[] TextPrefab = card.GetComponentsInChildren<TextMeshProUGUI>(); // R�cup�re tous les composants TextMeshProUGUI enfants de l'objet carte
            foreach (TextMeshProUGUI textMeshProUGUI in TextPrefab)
            {
                Debug.Log(textMeshProUGUI.name + " : " + textMeshProUGUI.text); // Affiche le nom et le texte du composant TextMeshProUGUI
            }
        }
    }

    // M�thode pour ajouter une carte sur le terrain
    public int AddOnBoard(string name)
    {
        int index = 0;
        for (int i = 0; i < cardInHandUI.Count; i++)
        {
            GameObject card = cardInHandUI.ElementAt(i); // R�cup�re l'objet carte en main � l'indice i
            card.GetComponentInChildren<TextMeshProUGUI>(); // R�cup�re le composant TextMeshProUGUI de l'objet carte
            TextMeshProUGUI[] TextPrefab = card.GetComponentsInChildren<TextMeshProUGUI>(); // R�cup�re tous les composants TextMeshProUGUI enfants de l'objet carte
            foreach (TextMeshProUGUI textMeshProUGUI in TextPrefab)
            {
                if (textMeshProUGUI.name == "Name") // V�rifie si le nom correspond au nom de la carte
                {
                    if (textMeshProUGUI.text == name) // Si le nom correspond, ajoute la carte sur le terrain
                    {
                        Debug.Log("carte ajouter sur le board UI : " + textMeshProUGUI.text);
                        card.transform.position = new Vector3(card.transform.position.x, -50, card.transform.position.z); // D�place la carte � une position sp�cifique sur l'interface utilisateur
                        cardOnBoardUI.Add(card); // Ajoute la carte sur le terrain (interface utilisateur)
                        cardInHandUI.RemoveAt(i); // Retire la carte de la main (interface utilisateur)
                    }
                }
            }
            Card carte = cardInHand.ElementAt(i); // R�cup�re la carte en main � l'indice i
            if (carte.card_name == name) // V�rifie si le nom de la carte correspond
            {
                Debug.Log("carte ajouter sur le board : " + carte.card_name);
                cardOnBoard.Add(carte); // Ajoute la carte sur le terrain
                cardInHand.RemoveAt(i); // Retire la carte de la main
                index = i;
            }
        }
        Debug.Log("index carte enlev� : " + index);
        return index; // Retourne l'indice de la carte enlev�e
    }

    // M�thode pour r�initialiser les donn�es en jeu
    public void ResetInGame()
    {
        cardInHand.Clear(); // Efface la liste des cartes en main
        cardOnBoard.Clear(); // Efface la liste des cartes sur le terrain
        cardOnBoardEnemy.Clear(); // Efface la liste des cartes sur le terrain de l'adversaire
        cardInHandUI.Clear(); // Efface la liste des objets repr�sentant les cartes en main (interface utilisateur)
        cardOnBoardUI.Clear(); // Efface la liste des objets repr�sentant les cartes sur le terrain (interface utilisateur)
        cardOnBoardEnemyUI.Clear(); // Efface la liste des objets repr�sentant les cartes sur le terrain de l'adversaire (interface utilisateur)
    }
}

// Classe repr�sentant une carte
public class Card
{
    public int Id; // Identifiant de la carte
    public string card_name; // Nom de la carte
    public string description; // Description de la carte
    public int pv; // Points de vie de la carte
    public int atk; // Points d'attaque de la carte
}
