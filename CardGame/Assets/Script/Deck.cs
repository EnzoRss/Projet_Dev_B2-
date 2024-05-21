using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Deck 
{
    int id;
    public string name;
    public List<Card> deck; 
    public List<Card> cardOnBoard;
    public List<Card> cardInHand;

    public void DrawCard()
    {
        int randomNumber = Random.Range(0, deck.Count);
        cardInHand.Add(deck.ElementAt(randomNumber));
        deck.RemoveAt(randomNumber);
    }
    public Deck(int id) 
    {
        this.id = id;
        deck = new List<Card>();
        cardOnBoard = new List<Card>();
        cardInHand = new List<Card>();
    }
}

public class Card
{
    public string Id_cards;
    public string card_name;
    public string description;
    public string pv;
    public string atk;
    public string Id_decks;
}




