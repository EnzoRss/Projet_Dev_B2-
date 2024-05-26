using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;

public class Deck 
{
    int id;
    public string name;
    public List<Card> deck;
    public List<Card> cardInHand;
    public List<Card> cardOnBoard;
    public List<GameObject> cardInHandUI;
    public List<GameObject> cardOnBoardUI;

    public List<Card> cardOnBoardEnemy;
    public List<GameObject> cardOnBoardEnemyUI;
    public  Card DrawCard()
    {  
        int randomNumber = Random.Range(1, deck.Count-1);
        Card cardTodeck = deck.ElementAt(randomNumber);
        cardInHand.Add(deck.ElementAt(randomNumber));
        deck.RemoveAt(randomNumber);
        return cardTodeck;
    }
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

    public void PrintCardUI()
    {
        foreach (GameObject card in cardInHandUI)
        {
            card.GetComponentInChildren<TextMeshProUGUI>();
            TextMeshProUGUI[] TextPrefab = card.GetComponentsInChildren<TextMeshProUGUI>();
            foreach (TextMeshProUGUI textMeshProUGUI in TextPrefab)
            {
                Debug.Log(textMeshProUGUI.name + " : " + textMeshProUGUI.text);
            }
        }
    }

    public int AddOnBoard(string name)
    {
        int index = 0;
        for (int i = 0; i< cardInHandUI.Count; i++)
        {
            GameObject card = cardInHandUI.ElementAt(i);
            card.GetComponentInChildren<TextMeshProUGUI>();
            TextMeshProUGUI[] TextPrefab = card.GetComponentsInChildren<TextMeshProUGUI>();
            foreach (TextMeshProUGUI textMeshProUGUI in TextPrefab)
            {
                if (textMeshProUGUI.name == "Name")
                {
                    if (textMeshProUGUI.text == name)
                    {
                        Debug.Log("carte ajouter sur le board UI : " + textMeshProUGUI.text);
                        card.transform.position = new Vector3(card.transform.position.x, -50, card.transform.position.z);
                        cardOnBoardUI.Add(card);
                        cardInHandUI.RemoveAt(i);

                    }
                }
            }
            Card carte = cardInHand.ElementAt(i);
            if (carte.card_name == name)
            {
                Debug.Log("carte ajouter sur le board : " + carte.card_name);
                cardOnBoard.Add(carte);
                cardInHand.RemoveAt(i);
                index = i;
            }
        }
        Debug.Log("index carte enlevé : "+ index);
        return index;
    }

    public void ResetInGame()
    {
        cardInHand.RemoveRange(cardInHand.Count, cardInHand.Count);
        cardOnBoard.RemoveRange(cardOnBoard.Count, cardOnBoard.Count);
        cardOnBoardEnemy.RemoveRange(cardOnBoardEnemy.Count, cardOnBoardEnemy.Count);
    }
}



public class Card
{
    public int Id;
    public string card_name;
    public string description;
    public int pv;
    public int atk;
    
}




