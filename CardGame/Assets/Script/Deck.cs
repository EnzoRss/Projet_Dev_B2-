using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Deck 
{
    int id;
    public string name;
    List<Card> deck; 
    List<Card> cardOnBoard;
    List<Card> cardInHand;

    public void DrawCard()
    {
        int randomNumber = Random.Range(0, deck.Count);
        cardInHand.Add(deck.ElementAt(randomNumber));
        deck.RemoveAt(randomNumber);
    }
}

public class Card
{
    int id;
    public string name;
    public string description;
    public int pv;
    public int atk;
}




